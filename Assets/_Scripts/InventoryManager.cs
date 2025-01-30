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

    [SerializeField] private Transform _lootBoxesTransform;
    [SerializeField] private Transform _charactersTransform;
    [SerializeField] private Transform _weaponsTransform;
    [SerializeField] private Transform _petsTransform;

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

        Transform[] inventoryItemTransforms = { _lootBoxesTransform, _charactersTransform, _weaponsTransform, _petsTransform };

        IEnumerable<InventoryItem> inventoryItems = inventoryItemTransforms.SelectMany(t => t.GetComponentsInChildren<InventoryItem>());

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

        Transform transform = inventoryItemPrefab.InventoryItemSO.Type switch
        {
            InventoryItemType.Character => _charactersTransform,
            InventoryItemType.Weapon => _weaponsTransform,
            InventoryItemType.Pet => _petsTransform,
            InventoryItemType.LootBox => _lootBoxesTransform,
            _ => throw new System.Exception("No type matches."),
        };

        InventoryItem spawnedInventoryItem = Instantiate(inventoryItemPrefab, transform);
        _inventoryItems.Add(spawnedInventoryItem);

        InventoryItem[] sorted = _inventoryItems.Where(i => i.InventoryItemSO.Type == inventoryItemPrefab.InventoryItemSO.Type).OrderBy(i => i.InventoryItemSO.Rarity).ToArray();

        for (int i = 0; i < sorted.Length; i++)
        {
            sorted[i].transform.SetSiblingIndex(i);
        }

        return spawnedInventoryItem;
    }

    public void RemoveInventoryItem(InventoryItem inventoryItem)
    {
        Destroy(inventoryItem.gameObject);
        _inventoryItems.Remove(inventoryItem);
    }

    public bool IsDuplicate(InventoryItem inventoryItemPrefab)
    {
        foreach (InventoryItem inventoryItem in _inventoryItems)
        {
            if (inventoryItem.InventoryItemSO == inventoryItemPrefab.InventoryItemSO)
            {
                return true;
            }
        }

        return false;
    }
}
