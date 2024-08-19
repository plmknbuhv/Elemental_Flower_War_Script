using ObjectPooling;
using UnityEngine;

public class PlayEffectFeedback : Feedback
{
    [SerializeField] private PoolingType effectType;
    [SerializeField] private float effectScale;
    
    public override void PlayFeedback()
    {
        EffectPlayer effect = PoolManager.Instance.Pop(effectType) as EffectPlayer;
        effect.transform.localScale *= effectScale;
        effect.SetPositionAndPlay(transform.position);
    }

    public override void StopFeedback()
    {
        
    }
}
