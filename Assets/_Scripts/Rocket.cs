using UnityEngine;

public class Rocket : Projectile
{
    [SerializeField] private GameObject _explosionEffectPrefab;
    [SerializeField] private float _explosionRange = 2;

    private void Start()
    {
        Invoke(nameof(Explode), DespawnTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Explode();
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(Damage);
            }

            if (collider.TryGetComponent(out KnockbackObject knockbackObject))
            {
                Vector2 direction = collider.transform.position - transform.position;
                knockbackObject.Knockback(direction.normalized, KnockbackForce, KnockbackDuration);
            }
        }

        GameObject explosionEffect = Instantiate(_explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(explosionEffect, 0.5f);

        Destroy(gameObject);
    }
}
