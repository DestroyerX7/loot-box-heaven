using System.Collections;
using UnityEngine;

public class ExplosionEnemy : MonoBehaviour
{
    private Rigidbody2D _rb;
    [SerializeField] private float _speed = 3;

    private Transform _playerTransform;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _detectionRange = 10;
    [SerializeField] private float _explosionDelay = 0.5f;
    [SerializeField] private float _knockbackForce;
    [SerializeField] private float _knockbackDuration;
    private bool _isAttacking;

    [SerializeField] private GameObject _explosionPrefab;

    private bool _isRunning;

    private Animator _animator;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        InvokeRepeating(nameof(ScanForPlayer), 0, 1);

        GameManager.Instance.OnPause.AddListener(() => { enabled = false; _rb.linearVelocity = Vector2.zero; });
        GameManager.Instance.OnUnpause.AddListener(() => enabled = true);
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

        _animator.SetTrigger("Attack");

        yield return new WaitForSeconds(_explosionDelay);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _attackRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
            }

            if (collider != null && collider.TryGetComponent(out KnockbackObject knockbackObject))
            {
                Vector2 direction = collider.transform.position - transform.position;
                knockbackObject.Knockback(direction, _knockbackForce, _knockbackDuration);
            }
        }

        GetComponent<LootDropper>().SetNumDrops(0);

        GetComponent<Health>().Die();

        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 1);
    }

    public void RemoveComponent()
    {
        Destroy(this);
    }
}
