using UnityEngine;

public class Pet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private float _runToRange = 1;
    [SerializeField] private float _stayRange = 3;
    private bool _isRunning;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _animator.SetBool("IsRunning", _isRunning);

        if (_playerRb == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, _playerRb.position);

        if (distance > _stayRange)
        {
            _isRunning = true;
        }
        else if (distance < _runToRange)
        {
            _isRunning = false;
        }

        if (_isRunning)
        {
            float speed = 5;
            transform.position = Vector2.MoveTowards(transform.position, _playerRb.position, speed * Time.deltaTime);
        }

        transform.localScale = _playerRb.position.x >= transform.position.x ? Vector3.one : new Vector3(-1, 1, 1);
    }

    public void SetPlayerRb(Rigidbody2D playerRb)
    {
        _playerRb = playerRb;
    }
}
