using DG.Tweening;
using TMPro;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }

    [SerializeField] private TextMeshPro _popupTextPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SpawnPopup(string text, Vector2 pos, float lifeTime, float speed = 1)
    {
        SpawnPopup(text, pos, lifeTime, Color.red, speed);
    }

    public void SpawnPopup(string text, Vector2 pos, float lifeTime, Color32 color, float speed = 1)
    {
        TextMeshPro popupText = Instantiate(_popupTextPrefab, pos, Quaternion.identity);

        popupText.text = text;
        popupText.color = color;

        float endYPos = pos.y + lifeTime * speed;
        popupText.transform.DOMoveY(endYPos, lifeTime).OnComplete(() => Destroy(popupText));
    }
}
