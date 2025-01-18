using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeWeapon : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _attackAction;

    [SerializeField] private float _damage = 10;
    [SerializeField] private float _attackRange = 1.5f;
    [SerializeField] private float _knockbackForce;
    [SerializeField] private float _knockbackDuration;

    private Animator _animator;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _attackAction = _playerInput.actions["Attack"];

        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_attackAction.triggered)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPos - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        transform.localScale = mouseWorldPos.x >= transform.position.x ? Vector3.one : new Vector3(1, -1, 1);
    }

    private void Attack()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, _attackRange);
        _animator.SetTrigger("Attack");

        if (hit.collider == null)
        {
            return;
        }

        if (hit.collider.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(_damage);
        }

        if (hit.collider.TryGetComponent(out KnockbackObject knockbackObject))
        {
            knockbackObject.Knockback(transform.right, _knockbackForce, _knockbackDuration);
        }
    }
}
