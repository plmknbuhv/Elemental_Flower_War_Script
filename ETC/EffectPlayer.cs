using System.Collections;
using ObjectPooling;
using UnityEngine;

public class EffectPlayer : PoolableMono
{
    public ParticleSystem[] _particles;
    
    public float _duration;
    private WaitForSeconds _particleDuration;
    
    private void Awake()
    {
        _particles = GetComponentsInChildren<ParticleSystem>();
        _duration = _particles[0].main.duration;
        _particleDuration = new WaitForSeconds(_duration);
    }
    
    public void SetPositionAndPlay(Vector3 position)
    {
        transform.position = position;
        for (int i = 0; i < _particles.Length; i++)
        {
            _particles[i].Play();
        }
        
        StartCoroutine(DelayAndGotoPoolCoroutine());
    }

    private IEnumerator DelayAndGotoPoolCoroutine()
    {
        yield return _particleDuration;
        PoolManager.Instance.Push(this);
    }

    public override void ResetItem()
    {
        _particles[0].Stop();
        _particles[0].Simulate(0);
    }
}