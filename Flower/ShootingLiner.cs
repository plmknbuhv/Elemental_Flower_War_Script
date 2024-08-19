using System;
using System.Collections;
using UnityEngine;

public class ShootingLiner : MonoBehaviour
{
    public LineRenderer TrajectoryLine { get; private set; }
    private Flower _owner;

    private bool _isCanAim = true;
    public bool isCore;

    public void Initialize(Flower flower)
    {
        TrajectoryLine = GetComponent<LineRenderer>();
        
        TrajectoryLine.positionCount = 2;
        TrajectoryLine.SetPosition(0, flower.transform.position + new Vector3(0,0.1f,0));
        TrajectoryLine.enabled = false;

        _owner = flower;

        if (isCore)
        {
            SetLine();
        }
    }

    private void OnDisable()
    {
        FlowerManager.Instance.knockBackEvent -= HandleKnockBackEvent;
    }

    public void SetLine()
    {
        FlowerManager.Instance.knockBackEvent += HandleKnockBackEvent;
        
        TrajectoryLine.SetPosition(0, _owner.transform.position + new Vector3(0,0.1f,0));
        TrajectoryLine.enabled = false;
    }

    private void HandleKnockBackEvent()
    {
        TrajectoryLine.enabled = false;
        _isCanAim = false;
        StartCoroutine(StopAimCoroutine());
    }

    private IEnumerator StopAimCoroutine()
    {
        if (!_isCanAim)
        {
            yield return new WaitForSeconds(1.5f);
            _isCanAim = true;
        }
    }

    public void AimTarget(Transform targetTrm, float radius)
    {
        if (targetTrm == null || !_isCanAim)
        {
            TrajectoryLine.enabled = false;
            return;
        }
        
        var targetPos = targetTrm.position;
        TrajectoryLine.enabled = true;
        TrajectoryLine.SetPosition(1, targetPos);
    }
}
