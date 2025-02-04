using DG.Tweening;
using UnityEngine;

public class BananaProjectile : Projectile
{
    [SerializeField] private float _throwDistance = 7.5f;
    [SerializeField] private float _spinSpeed = 500;

    private void Start()
    {
        Vector2 endPos = transform.position + transform.right * _throwDistance;
        float duration = (endPos - (Vector2)transform.position).magnitude / Velocity.magnitude;
        transform.DOMove(endPos, duration).SetLoops(2, LoopType.Yoyo).OnComplete(() => Destroy(gameObject));
        Rb.linearVelocity = Vector2.zero;

        Destroy(gameObject, DespawnTime);
    }

    private void Update()
    {
        transform.Rotate(0, 0, _spinSpeed * Time.deltaTime);
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
    }
}
