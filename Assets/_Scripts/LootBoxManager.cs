using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LootBoxManager : MonoBehaviour
{
    public static LootBoxManager Instance { get; private set; }

    private LootBox _selectedLootBox;
    [SerializeField] private Image _selectedLootBoxImage;
    [SerializeField] private Sprite _defaultLootBoxImage;

    [SerializeField] private Button _spinButton;
    [SerializeField] private MenuManager _menuManager;

    [SerializeField] private RectTransform _spinContent;
    [SerializeField] private float _endXPos = -2000;
    [SerializeField] private float _spinDuration = 1;

    private LootBoxSpinObject[] _lootBoxSpinObjects;

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
        _lootBoxSpinObjects = _spinContent.GetComponentsInChildren<LootBoxSpinObject>();
    }

    public void SelectLootBox(LootBox lootBox)
    {
        _selectedLootBox = lootBox;
        _spinButton.interactable = true;
        _menuManager.OpenMenu("LootBox");
        _spinContent.gameObject.SetActive(false);
        _selectedLootBoxImage.sprite = lootBox.InventoryItemSO.Sprite;
    }

    public void Open()
    {
        if (_selectedLootBox == null)
        {
            return;
        }

        foreach (LootBoxSpinObject lootBoxSpinObject in _lootBoxSpinObjects)
        {
            lootBoxSpinObject.SetSpriteImage(_selectedLootBox.Open().InventoryItemSO);
        }

        InventoryItem inventoryItemPrefab = _selectedLootBox.Open();
        InventoryManager.Instance.AddInventoryItem(inventoryItemPrefab);
        _selectedLootBox.SetStackCount(_selectedLootBox.StackCount - 1);
        _lootBoxSpinObjects[22].SetSpriteImage(inventoryItemPrefab.InventoryItemSO);

        if (_selectedLootBox.StackCount < 1)
        {
            _spinButton.interactable = false;
            _selectedLootBox = null;
            _selectedLootBoxImage.sprite = _defaultLootBoxImage;
        }

        _spinContent.DOKill();
        Spin();
    }

    private void Spin()
    {
        _spinContent.gameObject.SetActive(true);
        _spinContent.anchoredPosition = Vector2.zero;
        _spinContent.DOAnchorPosX(_endXPos, _spinDuration);
    }
}
