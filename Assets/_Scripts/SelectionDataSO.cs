using UnityEngine;

[CreateAssetMenu(menuName = "SelectionDataSO", fileName = "SelectionDataSO")]
public class SelectionDataSO : ScriptableObject
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
    [field: SerializeField] public InventoryItemSO InventoryItemSO { get; private set; }
}
