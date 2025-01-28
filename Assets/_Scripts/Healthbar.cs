using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Image _image;

    private void Start()
    {
        Health health = GetComponentInParent<Health>();
        health.OnHeal.AddListener(SetHeathbar);
        health.OnTakeDamage.AddListener(SetHeathbar);
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    public void SetHeathbar(float currentHealth, float maxHealth)
    {
        _image.fillAmount = currentHealth / maxHealth;
    }
}
