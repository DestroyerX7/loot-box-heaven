using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileWeapon : MonoBehaviour
{
    private enum FireMode
    {
        SemiAuto,
        FullAuto,
    }

    private PlayerInput _playerInput;
    private InputAction _attackAction;

    [SerializeField] private Projectile _projectilePrefab;

    [SerializeField] private Transform _shootTransform;
    [SerializeField] private float _damage = 10;
    [SerializeField] private float _projectileSpeed = 6;
    [SerializeField] private int _fireRate = 300;
    [SerializeField] private FireMode _fireMode;
    [SerializeField] private int _numProjectiles = 1;
    private bool _canShoot = true;

    [SerializeField] private float _knockbackForce = 25;
    [SerializeField] private float _knockbackDuration = 0.05f;

    [SerializeField] private float _maxSpreadAngle;

    private Animator _animator;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _attackAction = _playerInput.actions["Attack"];

        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_canShoot && _fireMode == FireMode.FullAuto && _attackAction.inProgress)
        {
            StartCoroutine(Attack());
        }
        else if (_canShoot && _fireMode == FireMode.SemiAuto && _attackAction.triggered)
        {
            StartCoroutine(Attack());
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
        _canShoot = false;

        for (int i = 0; i < _numProjectiles; i++)
        {
            float shootAngle = Random.Range(-_maxSpreadAngle, _maxSpreadAngle);
            Vector2 shootDirection = Quaternion.AngleAxis(shootAngle, Vector3.forward) * _shootTransform.right;
            float projectileRotationAngle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            Quaternion projectileRotation = Quaternion.Euler(0, 0, projectileRotationAngle);

            Projectile projectile = Instantiate(_projectilePrefab, _shootTransform.position, projectileRotation);
            projectile.SetVelcoity(shootDirection.normalized * _projectileSpeed);
            projectile.SetDamage(_damage);
            projectile.SetKnockbackStats(_knockbackForce, _knockbackDuration);
        }

        yield return new WaitForSeconds(60f / _fireRate);

        _canShoot = true;
    }
}
