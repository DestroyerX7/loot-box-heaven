using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemSO", menuName = "InventoryItemSO", order = 0)]
public class InventoryItemSO : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; }

    [field: SerializeField] public InventoryItemType Type { get; private set; }
    [field: SerializeField] public InventoryItemRarity Rarity { get; private set; }

    [field: SerializeField] public int MaxStackCount { get; private set; } = 10;
    [field: SerializeField] public bool CanHaveMultipleStacks { get; private set; }

    [field: SerializeField] public Sprite Sprite { get; private set; }
    [field: SerializeField] public Vector2 SpritePos { get; private set; }
    [field: SerializeField] public float SpriteRotationZ { get; private set; }
    [field: SerializeField] public float SpriteWidth { get; private set; }
    [field: SerializeField] public float SpriteHeight { get; private set; }
}

public enum InventoryItemType
{
    Character,
    Weapon,
    Pet,
    LootBox,
}

public enum InventoryItemRarity
{
    Common,
    Uncommon,
    Epic,
    Rare,
    Legendary,
}
