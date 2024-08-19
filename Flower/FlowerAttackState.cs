using ObjectPooling;
using UnityEngine;

public class FlowerAttackState : FlowerState
{
    public FlowerAttackState(Flower flower, FlowerStateMachine stateMachine) : base(flower, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateState()
    {
        base.UpdateState();
        
        if (CheckChangeToIdle()) return;

        CheckAttackAttempt();
    }

    private void CheckAttackAttempt()
    {
        if(!_flower.isCanFire) return;
        
        //_flower.ShootingLiner.AimTarget(_flower.targetTrm, _flower.detectRadius);
        
        if (_flower.lastAttackTime + _flower.attackCooldown < Time.time)
        {
            Attack();
            _flower.lastAttackTime = Time.time;
        }
    }

    private void Attack()
    {
        var aimDir = _flower.targetTrm.position - _flower.transform.position;
        var rotationAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        var targetRotate = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
        
        for (float i = 0.5f - (_flower.bulletCount * 0.5f); i < (_flower.bulletCount * 0.5f); i++)
        {
            Bullet bullet = PoolManager.Instance.Pop(PoolingType.Bullet) as Bullet;
            
            bullet.targetMat.SetColor(bullet.valueHash, FlowerManager.Instance.flowerElementDataListSo.elementDataDictionary[_flower.flowerType].color);
            
            bullet.transform.SetPositionAndRotation(
                _flower.transform.position, targetRotate * Quaternion.Euler(0f, 0f, 12.5f * i));
            bullet.knockBackPower = _flower.knockBackPower;
            bullet.attackDamage = _flower.attackDamage;
            bullet.flowerType = _flower.flowerType;
            bullet.radius = _flower.detectRadius;
            bullet.ownerTrm = _flower.transform;
            bullet.isDead = false;
            
            if (_flower.isGuidedBullet)
            {
                bullet.isGuided = true;
                bullet.targetTrm = _flower.targetTrm;
            }
        }
    }

    private bool CheckChangeToIdle()
    {
        Collider2D enemy = _flower.GetPlayerInRange();
        
        if (!enemy)
        {
            //_flower.ShootingLiner.AimTarget(null, _flower.detectRadius);
            _stateMachine.ChangeState(FlowerEnum.Idle);
            return true;
        }

        return false;
    }

    public override void Exit()
    {
        //_flower.ShootingLiner.AimTarget(null, _flower.detectRadius);
        _flower.lastAttackTime = Time.time;
    }
}
