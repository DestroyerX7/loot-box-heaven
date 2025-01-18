using System.Collections;
using DG.Tweening;
using UnityEngine;

public class Loot : MonoBehaviour
{
    private enum LootType
    {
        Coin,
        Gem,
        Health,
    }

    [SerializeField] private LootType _lootType;

    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_lootType == LootType.Coin)
            {
                StoreManager.Instance.Coins++;
            }
            else if (_lootType == LootType.Gem)
            {
                StoreManager.Instance.Gems++;
            }
            else
            {
                // other.GetComponent<Health>().
            }

            Destroy(gameObject);
        }
    }

    public void Bounce(Vector2 startVelocity, float stopTime)
    {
        StartCoroutine(BounceCorutine(startVelocity, stopTime));
    }

    private IEnumerator BounceCorutine(Vector2 startVelocity, float stopTime)
    {
        _rb.linearVelocity = startVelocity;

        yield return new WaitForSeconds(stopTime);

        _rb.linearVelocity = Vector2.zero;
        _rb.bodyType = RigidbodyType2D.Kinematic;
        transform.DOMoveY(transform.position.y + 0.25f, 1).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }
}
