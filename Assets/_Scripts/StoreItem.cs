using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    [SerializeField] private LootBoxSO _lootBoxSO;
    [SerializeField] private InventoryItem _inventoryItemPrefab;

    [field: SerializeField] public int CoinCost { get; private set; }
    [field: SerializeField] public int GemCost { get; private set; }

    [SerializeField] private TextMeshProUGUI _coinCostText;
    [SerializeField] private TextMeshProUGUI _gemCostText;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ShowInventoryItemPreviews);

        _coinCostText.text = CoinCost.ToString();
        _gemCostText.text = GemCost.ToString();
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

internal class TextMeshProGUI
{
}