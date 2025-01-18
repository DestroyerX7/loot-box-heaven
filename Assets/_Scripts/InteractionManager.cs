using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }

    [SerializeField] private GameObject _interactionUI;
    [SerializeField] private TextMeshProUGUI _interactionKeyText;
    [SerializeField] private TextMeshProUGUI _interactionMessageText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ShowInteractionUI(string key, string message)
    {
        _interactionUI.SetActive(true);
        _interactionKeyText.text = key;
        _interactionMessageText.text = message;
    }

    public void HideInteractionUI()
    {
        _interactionUI.SetActive(false);
    }
}
