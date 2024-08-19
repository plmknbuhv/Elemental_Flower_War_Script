using ObjectPooling;
using UnityEngine;

public abstract class FlowerState
{
    protected Flower _flower;
    protected FlowerStateMachine _stateMachine;
    
    public FlowerState(Flower flower, FlowerStateMachine stateMachine)
    {
        _flower = flower;
        _stateMachine = stateMachine;
    }
    
    public virtual void UpdateState()
    {
        _flower.expGetTimer += Time.deltaTime;

        if (_flower.expGetTimer >= _flower.expGetTime)
        {
            EXPManager.Instance.AddExp(_flower.expAmount);

            ExpEffect effect = PoolManager.Instance.Pop(PoolingType.Exp) as ExpEffect;
            effect.PlayEffect(_flower.transform.position);
            
            _flower.expGetTimer = 0;
        }
    }
    
    public virtual void Enter()
    {
        
    }

    public virtual void Exit()
    {
        
    }
}
