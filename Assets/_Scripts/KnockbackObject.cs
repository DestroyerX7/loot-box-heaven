using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class KnockbackObject : MonoBehaviour
{
    [SerializeField] private UnityEvent _onKnockbackStart;
    [SerializeField] private UnityEvent _onKnockbackEnd;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Knockback(Vector2 direction, float force, float duration)
    {
        StartCoroutine(KnockbackCoroutine(direction, force, duration));
    }

    private IEnumerator KnockbackCoroutine(Vector2 direction, float force, float duration)
    {
        _onKnockbackStart.Invoke();
        _rb.linearVelocity = direction.normalized * force;

        yield return new WaitForSeconds(duration);

        _rb.linearVelocity = Vector2.zero;
        _onKnockbackEnd.Invoke();
    }

    public void RemoveComponent()
    {
        Destroy(this);
    }
}
