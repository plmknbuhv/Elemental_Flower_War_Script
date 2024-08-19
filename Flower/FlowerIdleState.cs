using UnityEngine;

public class FlowerIdleState : FlowerState
{
    public FlowerIdleState(Flower flower, FlowerStateMachine stateMachine) : base(flower, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        Collider2D enemy = _flower.GetPlayerInRange();
        if (enemy)
        {
            _flower.targetTrm = enemy.transform;
            _stateMachine.ChangeState(FlowerEnum.Attack);
        }
    }
}
