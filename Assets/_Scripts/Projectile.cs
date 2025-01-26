using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Rigidbody2D Rb;
    protected Vector2 Velocity;

    [SerializeField] protected float DespawnTime = 10;

    protected float Damage;
    protected float KnockbackForce;
    protected float KnockbackDuration;

    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, DespawnTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(Damage);
        }

        if (other.gameObject.TryGetComponent(out KnockbackObject knockbackObject))
        {
            knockbackObject.Knockback(Velocity.normalized, KnockbackForce, KnockbackDuration);
        }

        Destroy(gameObject);
    }

    public void SetVelcoity(Vector2 velocity)
    {
        Rb.linearVelocity = velocity;
        Velocity = velocity;
    }

    public void SetDamage(float damage)
    {
        Damage = damage;
    }

    public void SetKnockbackStats(float knockbackForce, float knockbackDuration)
    {
        KnockbackForce = knockbackForce;
        KnockbackDuration = knockbackDuration;
    }
}
