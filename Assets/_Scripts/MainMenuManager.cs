using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }

    [SerializeField] private Image _selectedCharacterImage;
    [SerializeField] private Image _selectedWeaponImage;
    [SerializeField] private Image _selectedPetImage;

    [SerializeField] private Transform _inventoryItemPreviewContainer;
    [SerializeField] private GameObject _inventoryItemPreviewPrefab;

    private StoreItem _selectedStoreItem;
    [SerializeField] private Button _buyButton;

    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private Button _settingsButton;

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
        StoreManager.Instance.HideLootUI();

        Select(SelectionType.Character, SelectionManager.Instance.CharacterSelectionDataSO.InventoryItemSO);
        Select(SelectionType.Weapon, SelectionManager.Instance.WeaponSelectionDataSO.InventoryItemSO);

        if (SelectionManager.Instance.PetSelectionDataSO != null)
        {
            Select(SelectionType.Pet, SelectionManager.Instance.PetSelectionDataSO.InventoryItemSO);
        }

        _settingsButton.onClick.AddListener(ToggleSettings);
    }

    public void Play()
    {
        SceneManager.LoadScene("Village");
        StoreManager.Instance.ShowLootUI();
    }

    public void ShowLootUI()
    {
        StoreManager.Instance.ShowLootUI();
    }

    public void HideLootUI()
    {
        StoreManager.Instance.HideLootUI();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        {
            EditorApplication.isPlaying = false;
        }
#else
        {
            Application.Quit();
        }
#endif
    }

    public void Select(SelectionType selectionType, InventoryItemSO inventoryItemSO)
    {
        Image image = null;

        switch (selectionType)
        {
            case SelectionType.Character:
                image = _selectedCharacterImage;
                break;
            case SelectionType.Weapon:
                image = _selectedWeaponImage;
                break;
            case SelectionType.Pet:
                image = _selectedPetImage;
                break;
            default:
                return;
        }

        if (inventoryItemSO == null)
        {
            image.enabled = false;
            return;
        }

        image.sprite = inventoryItemSO.Sprite;
        image.rectTransform.anchoredPosition = inventoryItemSO.SpritePos;
        image.rectTransform.rotation = Quaternion.Euler(0, 0, inventoryItemSO.SpriteRotationZ);
        image.rectTransform.sizeDelta = new(inventoryItemSO.SpriteWidth, inventoryItemSO.SpriteHeight);
        image.enabled = true;
    }

    public void Buy()
    {
        if (_selectedStoreItem != null)
        {
            _selectedStoreItem.Buy();
        }
    }

    public void SelectStoreItem(StoreItem storeItem, InventoryItem[] inventoryItems)
    {
        _buyButton.interactable = storeItem.CoinCost <= StoreManager.Instance.Coins && storeItem.GemCost <= StoreManager.Instance.Gems;
        _selectedStoreItem = storeItem;

        foreach (Transform inventoryItemPreview in _inventoryItemPreviewContainer)
        {
            Destroy(inventoryItemPreview.gameObject);
        }

        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            InventoryItemSO inventoryItemSO = inventoryItem.InventoryItemSO;

            GameObject inventoryItemPreview = Instantiate(_inventoryItemPreviewPrefab, _inventoryItemPreviewContainer);

            Image image = inventoryItemPreview.GetComponentsInChildren<Image>().First(i => i.gameObject != inventoryItemPreview); // Make an inventoryItemPreview class
            image.sprite = inventoryItemSO.Sprite;
            image.rectTransform.anchoredPosition = inventoryItemSO.SpritePos;
            image.rectTransform.rotation = Quaternion.Euler(0, 0, inventoryItemSO.SpriteRotationZ);
            image.rectTransform.sizeDelta = new(inventoryItemSO.SpriteWidth, inventoryItemSO.SpriteHeight);
            image.enabled = true;
        }
    }

    private void ToggleSettings()
    {
        _settingsMenu.SetActive(!_settingsMenu.activeInHierarchy);
    }
}
