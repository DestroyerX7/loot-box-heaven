using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    [SerializeField] private SelectionDataSO _selectionDataSO;
    [SerializeField] private SelectionType _selectionType;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => SelectionManager.Instance.Select(_selectionType, _selectionDataSO));
    }
}
