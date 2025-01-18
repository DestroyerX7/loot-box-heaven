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
}
