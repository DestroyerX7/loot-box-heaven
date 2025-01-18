using UnityEngine;

public class Shuriken : Projectile
{
    [SerializeField] private float _spinSpeed = 500;

    private void Update()
    {
        transform.Rotate(0, 0, _spinSpeed * Time.deltaTime);
    }
}
