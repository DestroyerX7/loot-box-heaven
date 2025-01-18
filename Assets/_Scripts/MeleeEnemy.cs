using System.Collections;
using System.Linq;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _speed = 3;

    private Transform _playerTransform;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _detectionRange = 10;
    [SerializeField] private float _attackDelay = 0.5f;
    [SerializeField] private float _attackDuration = 0.5f;
    [SerializeField] private float _knockbackForce;
    [SerializeField] private float _knockbackDuration;
    private bool _isAttacking;

    private bool _isRunning;

    private Animator _animator;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        InvokeRepeating(nameof(ScanForPlayer), 0, 1);
    }

    private void Update()
    {
        _animator.SetBool("IsRunning", _isRunning);

        if (_isAttacking || _playerTransform == null)
        {
            _rb.linearVelocity = Vector2.zero;
            _isRunning = false;
            return;
        }

        Vector2 direction = _playerTransform.position - transform.position;
        _rb.linearVelocity = direction.normalized * _speed;
        _isRunning = true;

        Vector3 rotation = _playerTransform.position.x >= transform.position.x ? Vector3.zero : new(0, 180, 0);
        transform.rotation = Quaternion.Euler(rotation);

        if (direction.magnitude <= _attackRange)
        {
            StartCoroutine(Attack());
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

    private IEnumerator Attack()
    {
        _isAttacking = true;

        yield return new WaitForSeconds(_attackDelay);

        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, _attackRange, LayerMask.GetMask("Player"));

        if (playerCollider != null && playerCollider.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
        }

        if (playerCollider != null && playerCollider.TryGetComponent(out KnockbackObject knockbackObject))
        {
            Vector2 direction = _playerTransform.position - transform.position;
            knockbackObject.Knockback(direction, _knockbackForce, _knockbackDuration);
        }

        _animator.SetTrigger("Attack");

        yield return new WaitForSeconds(_attackDuration);

        _isAttacking = false;
    }

    public void RemoveComponent()
    {
        Destroy(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
