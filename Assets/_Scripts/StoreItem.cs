using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    [SerializeField] private InventoryItem _inventoryItemPrefab;
    [SerializeField] private int _coinCost;
    [SerializeField] private int _gemCost;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Buy);
    }

    private void Buy()
    {
        if (_coinCost > StoreManager.Instance.Coins || _gemCost > StoreManager.Instance.Gems)
        {
            return;
        }

        StoreManager.Instance.Coins -= _coinCost;
        StoreManager.Instance.Gems -= _gemCost;

        InventoryManager.Instance.AddInventoryItem(_inventoryItemPrefab);
    }
}
