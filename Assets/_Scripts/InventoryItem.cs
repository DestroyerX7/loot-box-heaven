using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public static Dictionary<InventoryItemRarity, Color32> RarityToColor { get; } = new()
    {
        { InventoryItemRarity.Common, new(128, 128, 128, 255) },
        { InventoryItemRarity.Uncommon, new(0, 255, 0, 255) },
        { InventoryItemRarity.Epic, new(0, 128, 255, 255) },
        { InventoryItemRarity.Rare, new(224, 0, 255, 255) },
        { InventoryItemRarity.Legendary, new(255, 224, 0, 255) },
    };

    [field: SerializeField] public InventoryItemSO InventoryItemSO { get; private set; }
    [field: SerializeField] public int StackCount { get; private set; } = 1;

    [SerializeField] private TextMeshProUGUI _stackCountText;

    protected virtual void Start()
    {
        SetStackCount(StackCount);
    }

    public void SetStackCount(int stackCount)
    {
        StackCount = stackCount;

        if (StackCount < 1)
        {
            InventoryManager.Instance.RemoveInventoryItem(this);
            return;
        }

        _stackCountText.text = StackCount > 1 ? StackCount.ToString() : "";
    }
}
