using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    [SerializeField] private LootBoxSO _lootBoxSO;
    [SerializeField] private InventoryItem _inventoryItemPrefab;

    [field: SerializeField] public int CoinCost { get; private set; }
    [field: SerializeField] public int GemCost { get; private set; }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowInventoryItemPreviews);
    }

    public void Buy()
    {
        if (CoinCost > StoreManager.Instance.Coins || GemCost > StoreManager.Instance.Gems)
        {
            return;
        }

        StoreManager.Instance.Coins -= CoinCost;
        StoreManager.Instance.Gems -= GemCost;

        InventoryManager.Instance.AddInventoryItem(_inventoryItemPrefab);
    }

    private void ShowInventoryItemPreviews()
    {
        InventoryItem[] inventoryItems = _lootBoxSO.RarityGroups.SelectMany(r => r.InventoryItemPrefabs).ToArray();
        MainMenuManager.Instance.SelectStoreItem(this, inventoryItems);
    }
}
