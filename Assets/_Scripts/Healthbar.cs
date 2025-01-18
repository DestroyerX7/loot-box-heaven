using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public void SetHeathbar(float currentHealth, float maxHealth)
    {
        print(currentHealth / maxHealth);
    }
}
