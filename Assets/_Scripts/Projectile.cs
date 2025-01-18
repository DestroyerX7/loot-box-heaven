using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Vector2 _velocity;

    [SerializeField] private float _despawnTime = 10;

    private float _damage;
    private float _knockbackForce;
    private float _knockbackDuration;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, _despawnTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
        }

        if (other.gameObject.TryGetComponent(out KnockbackObject knockbackObject))
        {
            knockbackObject.Knockback(_velocity.normalized, _knockbackForce, _knockbackDuration);
        }

        Destroy(gameObject);
    }

    public void SetVelcoity(Vector2 velocity)
    {
        _rb.linearVelocity = velocity;
        _velocity = velocity;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    public void SetKnockbackStats(float knockbackForce, float knockbackDuration)
    {
        _knockbackForce = knockbackForce;
        _knockbackDuration = knockbackDuration;
    }
}
