using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LootBox : InventoryItem
{
    [System.Serializable]
    private struct RarityGroup
    {
        public InventoryItem[] InventoryItemPrefabs;
        public int MinInclusive;
        public int MaxExclusive;
    }

    [SerializeField] private RarityGroup[] _rarityGroups;

    protected override void Start()
    {
        base.Start();
        GetComponent<Button>().onClick.AddListener(() => LootBoxManager.Instance.SelectLootBox(this));
    }

    public InventoryItem Open()
    {
        int randNum = Random.Range(0, 100);
        InventoryItem[] inventoryItemPrefabs = _rarityGroups.First(r => randNum >= r.MinInclusive && randNum < r.MaxExclusive).InventoryItemPrefabs;
        return inventoryItemPrefabs[Random.Range(0, inventoryItemPrefabs.Length)];
    }
}
