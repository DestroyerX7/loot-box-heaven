using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemsTree : MonoBehaviour
{
    [SerializeField] private InventoryItem[] _inventoryItemPrefabs;

    [SerializeField] private GameObject _inventoryItemPreviewPrefab;

    private void OnEnable()
    {
        ShowInventoryItems();
    }

    private void ShowInventoryItems()
    {
        foreach (Transform inventoryItemPreview in transform)
        {
            Destroy(inventoryItemPreview.gameObject);
        }

        _inventoryItemPrefabs = _inventoryItemPrefabs.OrderBy(i => i.InventoryItemSO.Rarity).ToArray();

        foreach (InventoryItem inventoryItemPrefab in _inventoryItemPrefabs)
        {
            InventoryItemSO inventoryItemSO = inventoryItemPrefab.InventoryItemSO;

            if (inventoryItemSO.Type == InventoryItemType.LootBox)
            {
                continue;
            }

            GameObject inventoryItemPreview = Instantiate(_inventoryItemPreviewPrefab, transform);

            Image image = inventoryItemPreview.GetComponentsInChildren<Image>().First(i => i.gameObject != inventoryItemPreview); // Make an inventoryItemPreview class
            image.sprite = inventoryItemSO.Sprite;
            image.rectTransform.anchoredPosition = inventoryItemSO.SpritePos;
            image.rectTransform.rotation = Quaternion.Euler(0, 0, inventoryItemSO.SpriteRotationZ);
            image.rectTransform.sizeDelta = new(inventoryItemSO.SpriteWidth, inventoryItemSO.SpriteHeight);
            image.enabled = true;

            bool isOwned = InventoryManager.Instance.IsDuplicate(inventoryItemPrefab);
            if (!isOwned)
            {
                image.color = Color.black;
            }
        }
    }
}
