using System;
using System.Collections;
using ObjectPooling;
using UnityEngine;

public enum FlowerElementType
{
    Fire,
    Ice,
    Water,
    Electric,
    Normal
}

public class Flower : PoolableMono
{
    public FlowerElementType flowerType; 
    
    public FlowerStateMachine stateMachine;
    
    [Header("Attack Settings")] 
    public float attackCooldown = 1.1f;
    public float detectRadius = 3.8f;
    public float knockBackPower;
    public int bulletCount = 1;
    public int attackDamage;
    
    public int expAmount = 1;
    public float expGetTime = 5;
    public float expGetTimer;
    
    public ContactFilter2D contactFilter;
    
    public Transform targetTrm = null;
    [HideInInspector] public float lastAttackTime;
    
    private Collider2D[] _colliders;

    public bool isGuidedBullet;
    public bool isCanFire;
    public bool isCore;

    public Material FlowerMat { get; private set; }
    
    #region Component Regeion

    public SpriteRenderer SpriteRenderer { get; private set; }
    public FlowerUpgrade FlowerUpgrade { get; private set; }
    //public ShootingLiner ShootingLiner { get; private set; }
    
    #endregion
    
    private void Awake()
    {
        _colliders = new Collider2D[1];
        
        SetStateMachine();

        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        FlowerUpgrade = GetComponent<FlowerUpgrade>();
            
        //ShootingLiner = GetComponentInChildren<ShootingLiner>();
        //ShootingLiner.Initialize(this);

        FlowerMat = SpriteRenderer.material;

        if (isCore)
        {
            SetUpFlower(flowerType);
        }
    }
    
    private void Update()
    {
        if(!isCanFire) return;
        
        stateMachine.CurrentState.UpdateState();
    }

    public void SetUpFlower(FlowerElementType flowerElementType)
    {
        var flowerData = FlowerManager.Instance.flowerElementDataListSo.elementDataDictionary[flowerElementType];
        knockBackPower = flowerData.knockBackPower;
        attackDamage = flowerData.damage;
        flowerType = flowerData.flowerType;
        SpriteRenderer.sprite = flowerData.sprite;
        bulletCount = 1;
        detectRadius = 3.8f;
        
        FlowerManager.Instance.flowerList.Add(this);
        FlowerManager.Instance.knockBackEvent += HandleKnockBackKeyEvent;
    }

    private void HandleKnockBackKeyEvent()
    {
        isCanFire = false;
        StartCoroutine(StopFireCoroutine());
    }

    private IEnumerator StopFireCoroutine()
    {
        if (!isCanFire)
        {
            lastAttackTime = Time.time;
            yield return new WaitForSeconds(1.5f);
            lastAttackTime = Time.time;
            isCanFire = true;
        }
    }

    private void SetStateMachine()
    {
        stateMachine = new FlowerStateMachine();

        stateMachine.AddState(FlowerEnum.Idle, new FlowerIdleState(this, stateMachine));
        stateMachine.AddState(FlowerEnum.Attack, new FlowerAttackState(this, stateMachine));
        
        stateMachine.Initialize(FlowerEnum.Idle, this);
    }
    
    public Collider2D GetPlayerInRange()
    {
        int count = Physics2D.OverlapCircle(transform.position, detectRadius, contactFilter, _colliders);

        return count > 0 ? _colliders[0] : null;
    }

    public bool UpgradeFlower(UpgradeEnum upgradeEnum)
    {
        return FlowerUpgrade.CheckUpgrade(upgradeEnum);
    }
    
    public Coroutine DelayCall(float time, Action CallbackAction)
    {
        return StartCoroutine(DelayCoroutine(time, CallbackAction));
    }

    private IEnumerator DelayCoroutine(float time, Action CallbackAction)
    {
        yield return new WaitForSeconds(time);
        CallbackAction?.Invoke();
    }

    public override void ResetItem()
    {
        FlowerManager.Instance.flowerList.Remove(this);
        isGuidedBullet = false; 
        isCanFire = false;
        expAmount = 1;
    }
    
#if UNITY_EDITOR
    
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
        Gizmos.color = Color.white;
    }
    
#endif
}
