using UnityEngine;

[CreateAssetMenu(menuName = "LootBoxSO", fileName = "LootBoxSO")]
public class LootBoxSO : ScriptableObject
{
    [field: SerializeField] public int MaxRange { get; private set; } = 100;
    [field: SerializeField] public RarityGroup[] RarityGroups { get; private set; }
}

[System.Serializable]
public struct RarityGroup
{
    public InventoryItem[] InventoryItemPrefabs;
    public int MinInclusive;
    public int MaxExclusive;
}
