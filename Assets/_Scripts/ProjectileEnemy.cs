using System.Collections;
using System.Linq;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    [SerializeField] private Projectile _projectilePrefab;

    private Rigidbody2D _rb;
    [SerializeField] private float _speed = 2;

    private Transform _playerTransform;
    [SerializeField] private float _projectileSpeed = 6;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _attackRange = 5;
    [SerializeField] private float _followDistance = 4;
    [SerializeField] private float _detectionRange = 10;
    [SerializeField] private float _attackCooldown = 1;
    [SerializeField] private float _attackDuration = 0.5f;
    private bool _canAttack = true;

    [SerializeField] private float _knockbackForce = 25;
    [SerializeField] private float _knockbackDuration = 0.05f;

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

        if (_playerTransform == null)
        {
            _rb.linearVelocity = Vector2.zero;
            _isRunning = false;
            return;
        }

        Vector2 direction = _playerTransform.position - transform.position;

        if (direction.magnitude > _attackRange)
        {
            _rb.linearVelocity = direction.normalized * _speed;
            _isRunning = true;
        }
        else if (direction.magnitude < _followDistance)
        {
            _rb.linearVelocity = -direction.normalized * _speed;
            _isRunning = true;
        }
        else
        {
            _rb.linearVelocity = Vector2.zero;
            _isRunning = false;
        }

        Vector3 rotation = _playerTransform.position.x >= transform.position.x ? Vector3.zero : new(0, 180, 0);
        transform.rotation = Quaternion.Euler(rotation);

        if (_canAttack && direction.magnitude <= _attackRange)
        {
            StartCoroutine(Attack());
        }
    }

    private void ScanForPlayer()
    {
        Collider2D playerCollider = Physics2D.OverlapCircleAll(transform.position, _detectionRange, LayerMask.GetMask("Player")).FirstOrDefault();

        if (playerCollider != null)
        {
            _playerTransform = playerCollider.transform;
        }
    }

    private IEnumerator Attack()
    {
        _canAttack = false;

        Vector2 direction = _playerTransform.position - transform.position;
        Projectile projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        projectile.SetVelcoity(direction.normalized * _projectileSpeed);
        projectile.SetDamage(_damage);
        projectile.SetKnockbackStats(_knockbackForce, _knockbackDuration);

        _animator.SetTrigger("Attack");

        yield return new WaitForSeconds(_attackDuration + _attackCooldown);

        _canAttack = true;
    }

    public void RemoveComponent()
    {
        Destroy(this);
    }
}
