using ObjectPooling;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D.Animation;

public class Bullet : PoolableMono
{
    public UnityEvent OnFireEvent;

    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float deadTime = 10f;
    [SerializeField] private float straightTime = 0.275f;
    [SerializeField] private float deadTimer;

    public FlowerElementType flowerType;
    public float knockBackPower;
    public Transform targetTrm;
    public Transform ownerTrm;
    public int attackDamage;
    public bool isGuided;
    public float radius;
    public bool isDead;

    public Material targetMat;
    public readonly int valueHash = Shader.PropertyToID("_EmissionColor");

    public Animator Animator { get; private set; }
    public SpriteLibrary SpriteLibrary { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }

    public readonly int DeadHash = Animator.StringToHash("isDead");

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        SpriteLibrary = GetComponent<SpriteLibrary>();
        SpriteRenderer = GetComponent<SpriteRenderer>();

        targetMat = SpriteRenderer.material;
    }

    private void Update()
    {
        deadTimer += Time.deltaTime;
        
        transform.position += transform.right * (moveSpeed * Time.deltaTime);

        if (deadTimer >= deadTime || Vector3.Distance(transform.position, ownerTrm.position) >= radius)
        {
            Animator.SetBool(DeadHash, true);
        }

        if (isGuided && !targetTrm.gameObject.activeSelf)
        {
            isGuided = false;
        }
        
        if (isGuided && deadTimer >= straightTime && targetTrm.gameObject.activeSelf)
        {
            var aimDir = targetTrm.position - transform.position;
            var rotationAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
        }
    }

    private void DestroyBullet()
    {
        deadTimer = 0;
        isDead = true;
        PlayHitEffect();

        PoolManager.Instance.Push(this);
    }

    private void PlayHitEffect()
    {
        EffectPlayer effect;

        switch (flowerType)
        {
            case FlowerElementType.Fire:
                effect = PoolManager.Instance.Pop(PoolingType.FireHitEffect) as EffectPlayer;
                break;
            case FlowerElementType.Ice:
                effect = PoolManager.Instance.Pop(PoolingType.SlowEffect) as EffectPlayer;
                break;
            case FlowerElementType.Water:
                effect = PoolManager.Instance.Pop(PoolingType.WaterHitEffect) as EffectPlayer;
                break;
            case FlowerElementType.Electric:
                effect = PoolManager.Instance.Pop(PoolingType.StunEffect) as EffectPlayer;
                break;
            case FlowerElementType.Normal:
                effect = PoolManager.Instance.Pop(PoolingType.HitEffect) as EffectPlayer;
                break;
            default:
                effect = PoolManager.Instance.Pop(PoolingType.HitEffect) as EffectPlayer;
                break;
        }

        effect.SetPositionAndPlay(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (other.TryGetComponent(out Agent enemy))
        {
            float damageMultiplier = enemy.StateData.attribWeaknessType == flowerType ? 2 : 1;
            int damage = Mathf.RoundToInt(attackDamage * damageMultiplier);

            enemy.HealthCompo.TakeDamage(damage, knockBackPower);
            if (flowerType == FlowerElementType.Electric)
            {
                enemy.Debuff(100f, Color.yellow, 0.5f);
            }
            else if (flowerType == FlowerElementType.Ice)
            {
                enemy.Debuff(45f, Color.blue, 0.8f);
            }

            DestroyBullet();
        }
    }

    public override void ResetItem()
    {
        isGuided = false;
        targetTrm = null;

        OnFireEvent?.Invoke();
    }
}
