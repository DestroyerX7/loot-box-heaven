using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flamethrower : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _attackAction;

    [SerializeField] private ParticleSystem _flameEffect;

    [SerializeField] private float _damage = 10;
    [SerializeField] private float _range = 7;
    [SerializeField] private int _fireRate = 300;
    private bool _canShoot = true;

    [SerializeField] private float _knockbackForce = 5;
    [SerializeField] private float _knockbackDuration = 0.05f;

    [SerializeField] private LayerMask _damageLayers;

    private Animator _animator;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _attackAction = _playerInput.actions["Attack"];

        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_attackAction.inProgress && _canShoot)
        {
            StartCoroutine(Attack());
        }

        if (_attackAction.triggered)
        {
            _flameEffect.Play();
        }
        else if (!_attackAction.inProgress)
        {
            _flameEffect.Stop();
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

    private IEnumerator Attack()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseWorldPos - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position + _range / 2 * transform.right, new(_range, 1), angle, _damageLayers);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
            }

            if (collider.TryGetComponent(out KnockbackObject knockbackObject))
            {
                knockbackObject.Knockback(transform.right, _knockbackForce, _knockbackDuration);
            }
        }

        _canShoot = false;

        yield return new WaitForSeconds(60f / _fireRate);

        _canShoot = true;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(transform.position + _range / 2 * transform.right, 1);
    //     Gizmos.DrawWireCube(transform.position + _range / 2 * transform.right, new(1, _range));
    // }
}
