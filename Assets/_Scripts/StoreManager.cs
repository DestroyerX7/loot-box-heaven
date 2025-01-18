using TMPro;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    private struct StoreSaveData
    {
        public int Coins;
        public int Gems;

        public StoreSaveData(int coins, int gems)
        {
            Coins = coins;
            Gems = gems;
        }
    }

    public static StoreManager Instance { get; private set; }

    public int Coins;
    public int Gems;

    private Canvas _canvas;
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _gemsText;

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
        StoreSaveData storeSaveData = SaveDataManager.LoadData<StoreSaveData>("/store.json");
        Coins = storeSaveData.Coins;
        Gems = storeSaveData.Gems;

        _canvas = GetComponent<Canvas>();
    }

    private void OnDestroy()
    {
        StoreSaveData storeSaveData = new(Coins, Gems);
        storeSaveData.SaveData("/store.json");
    }

    private void Update()
    {
        if (_canvas.enabled)
        {
            _coinsText.text = Coins.ToString();
            _gemsText.text = Gems.ToString();
        }
    }

    public void ShowLootUI()
    {
        _canvas.enabled = true;
    }

    public void HideLootUI()
    {
        _canvas.enabled = false;
    }
}
