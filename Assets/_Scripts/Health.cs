using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 100;
    private float _currentHealth;

    public UnityEvent<float, float> OnHeal;
    public UnityEvent<float, float> OnTakeDamage;
    public UnityEvent OnDie;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public void Heal(float healAmount)
    {
        _currentHealth = Mathf.Min(_currentHealth + healAmount, _maxHealth);
        OnHeal.Invoke(_currentHealth, _maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (_currentHealth <= 0)
        {
            return;
        }

        _currentHealth -= damage;
        OnTakeDamage.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDie.Invoke();
    }

    public void Despawn(float time)
    {
        Destroy(gameObject, time);
    }
}

public interface IDamageable
{
    public void TakeDamage(float damage);
}
