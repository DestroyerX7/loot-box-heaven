using DG.Tweening;
using UnityEngine;

public class JumpEnemy : MonoBehaviour
{
    private Transform _playerTransform;

    [SerializeField] private float _detectionRange = 15;
    [SerializeField] private float _jumpDistance = 3;
    [SerializeField] private float _jumpPower = 0.5f;
    [SerializeField] private float _jumpDuration = 1;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        InvokeRepeating(nameof(ScanForPlayer), 0, 1);
        InvokeRepeating(nameof(JumpTowardsPlayer), 0, 1);
    }

    private void OnDisable()
    {
        transform.DOKill();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(10);
            }

            if (other.gameObject.TryGetComponent(out KnockbackObject knockbackObject))
            {
                Vector2 direction = _playerTransform.position - transform.position;
                knockbackObject.Knockback(direction, 10, 0.05f);
            }

            Destroy(gameObject);
        }
    }

    private void ScanForPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, _detectionRange, LayerMask.GetMask("Player"));

        if (playerCollider != null)
        {
            _playerTransform = playerCollider.transform;
        }
    }

    private void JumpTowardsPlayer()
    {
        if (_playerTransform == null)
        {
            return;
        }

        Vector2 direction = _playerTransform.position - transform.position;

        Vector3 jumpPos = direction.magnitude < _jumpDistance ? direction : direction.normalized * _jumpDistance;

        _animator.SetTrigger("Jump");
        transform.DOJump(transform.position + jumpPos, _jumpPower, 1, _jumpDuration);
    }

    public void RemoveComponent()
    {
        Destroy(this);
    }
}
