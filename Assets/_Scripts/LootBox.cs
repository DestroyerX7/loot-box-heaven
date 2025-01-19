using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LootBox : InventoryItem
{
    [field: SerializeField] public LootBoxSO LootBoxSO { get; private set; }

    protected override void Start()
    {
        base.Start();
        GetComponent<Button>().onClick.AddListener(() => LootBoxManager.Instance.SelectLootBox(this));
    }

    public InventoryItem Open()
    {
        int randNum = Random.Range(0, 100);
        InventoryItem[] inventoryItemPrefabs = LootBoxSO.RarityGroups.First(r => randNum >= r.MinInclusive && randNum < r.MaxExclusive).InventoryItemPrefabs;
        return inventoryItemPrefabs[Random.Range(0, inventoryItemPrefabs.Length)];
    }
}
