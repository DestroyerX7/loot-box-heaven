using UnityEngine;

[CreateAssetMenu(menuName = "LootBoxSO", fileName = "LootBoxSO")]
public class LootBoxSO : ScriptableObject
{
    [field: SerializeField] public RarityGroup[] RarityGroups { get; private set; }
}

[System.Serializable]
public struct RarityGroup
{
    public InventoryItem[] InventoryItemPrefabs;
    public int MinInclusive;
    public int MaxExclusive;
}
