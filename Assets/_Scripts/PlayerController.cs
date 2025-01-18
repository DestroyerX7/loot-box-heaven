using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _moveAction;

    private Rigidbody2D _rb;
    [SerializeField] private float _speed = 5;

    private Animator _animator;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];

        _rb = GetComponent<Rigidbody2D>();

        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        _rb.linearVelocity = moveInput * _speed;

        _animator.SetBool("IsRunning", moveInput != Vector2.zero);

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 rotation = mouseWorldPos.x >= transform.position.x ? Vector3.zero : new(0, 180, 0);
        transform.rotation = Quaternion.Euler(rotation);
    }
}
