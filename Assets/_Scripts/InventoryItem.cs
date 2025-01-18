using TMPro;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
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
