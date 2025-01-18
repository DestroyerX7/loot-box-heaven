using UnityEngine;
using UnityEngine.UI;

public class LootBoxSpinObject : MonoBehaviour
{
    [SerializeField] private Image _spriteImage;

    public void SetSpriteImage(Sprite sprite, Vector2 spritePos, float spriteRotationZ, float spriteWidth, float spriteHeight)
    {
        _spriteImage.sprite = sprite;
        _spriteImage.rectTransform.anchoredPosition = spritePos;
        _spriteImage.rectTransform.rotation = Quaternion.Euler(0, 0, spriteRotationZ);
        _spriteImage.rectTransform.sizeDelta = new(spriteWidth, spriteHeight);
    }

    public void SetSpriteImage(InventoryItemSO inventoryItemSO)
    {
        SetSpriteImage(inventoryItemSO.Sprite, inventoryItemSO.SpritePos, inventoryItemSO.SpriteRotationZ, inventoryItemSO.SpriteWidth, inventoryItemSO.SpriteHeight);
    }
}
