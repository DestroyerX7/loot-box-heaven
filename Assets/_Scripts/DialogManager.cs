using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    private Canvas _canvas;
    [SerializeField] private Image _faceImage;
    [SerializeField] private TextMeshProUGUI _dialogText;

    private string[] _currentDialog;
    private int _currentDialogIndex;

    private bool _wasActivededThisFrame;

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

    private void Start()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void Update()
    {
        if (_currentDialog != null && Input.GetKeyDown(KeyCode.F) && !_wasActivededThisFrame)
        {
            ShowNextDialog();
        }

        _wasActivededThisFrame = false;
    }

    public void StartDialog(Sprite faceSprite, params string[] dialog)
    {
        _faceImage.sprite = faceSprite;
        _currentDialog = dialog;
        _currentDialogIndex = 0;
        _wasActivededThisFrame = true;

        _canvas.enabled = true;
        ShowNextDialog();
    }

    private void ShowNextDialog()
    {
        if (_currentDialogIndex >= _currentDialog.Length)
        {
            _canvas.enabled = false;
            return;
        }

        _dialogText.text = _currentDialog[_currentDialogIndex];
        _currentDialogIndex++;
    }
}
