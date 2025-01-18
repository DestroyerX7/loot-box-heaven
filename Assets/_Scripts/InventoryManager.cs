using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [System.Serializable]
    private struct InventorySaveData
    {
        public InventoryItemSaveData[] InventoryItemSaveDatas;
    }

    [System.Serializable]
    private struct InventoryItemSaveData
    {
        public string InventoryItemName;
        public int StackCount;

        public InventoryItemSaveData(string inventoryItemName, int stackCount)
        {
            InventoryItemName = inventoryItemName;
            StackCount = stackCount;
        }
    }

    public static InventoryManager Instance { get; private set; }

    private List<InventoryItem> _inventoryItems = new();

    [SerializeField] private Transform _inventoryItemsContainer;

    [SerializeField] private InventoryItem[] _inventoryItemPrefabs; // Maybe make this an SO later
    [SerializeField] private Dictionary<string, InventoryItem> _inventoryItemPrefabsDictionary = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetupDictionary();

        InventoryItem[] inventoryItems = _inventoryItemsContainer.GetComponentsInChildren<InventoryItem>();

        InventorySaveData inventorySaveData = SaveDataManager.LoadData<InventorySaveData>("/inventory.json");

        if (inventorySaveData.InventoryItemSaveDatas != null)
        {
            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                Destroy(inventoryItem.gameObject);
            }

            foreach (InventoryItemSaveData inventoryItemSaveData in inventorySaveData.InventoryItemSaveDatas)
            {
                InventoryItem inventoryItemPrefab = _inventoryItemPrefabsDictionary[inventoryItemSaveData.InventoryItemName];
                InventoryItem inventoryItem = AddInventoryItem(inventoryItemPrefab);
                inventoryItem.SetStackCount(inventoryItemSaveData.StackCount);
            }
        }
        else
        {
            foreach (InventoryItem inventoryItem in inventoryItems)
            {
                _inventoryItems.Add(inventoryItem);
            }
        }
    }

    private void OnDestroy()
    {
        InventoryItemSaveData[] inventoryItemSaveDatas = _inventoryItems.Select(i => new InventoryItemSaveData(i.InventoryItemSO.name, i.StackCount)).ToArray();

        InventorySaveData inventorySaveData = new()
        {
            InventoryItemSaveDatas = inventoryItemSaveDatas,
        };

        inventorySaveData.SaveData("/inventory.json");
    }

    private void SetupDictionary()
    {
        foreach (InventoryItem inventoryItemPrefab in _inventoryItemPrefabs)
        {
            _inventoryItemPrefabsDictionary.Add(inventoryItemPrefab.InventoryItemSO.name, inventoryItemPrefab);
        }
    }

    // Maybe make an add amount later
    public InventoryItem AddInventoryItem(InventoryItem inventoryItemPrefab)
    {
        foreach (InventoryItem inventoryItem in _inventoryItems)
        {
            if (inventoryItem.InventoryItemSO == inventoryItemPrefab.InventoryItemSO)
            {
                if (inventoryItem.StackCount < inventoryItem.InventoryItemSO.MaxStackCount)
                {
                    inventoryItem.SetStackCount(inventoryItem.StackCount + 1);
                    return inventoryItem;
                }
                else if (!inventoryItem.InventoryItemSO.CanHaveMultipleStacks)
                {
                    return null;
                }
            }
        }

        InventoryItem spawnedInventoryItem = Instantiate(inventoryItemPrefab, _inventoryItemsContainer);
        _inventoryItems.Add(spawnedInventoryItem);
        return spawnedInventoryItem;
    }

    public void RemoveInventoryItem(InventoryItem inventoryItem)
    {
        Destroy(inventoryItem.gameObject);
        _inventoryItems.Remove(inventoryItem);
    }
}
