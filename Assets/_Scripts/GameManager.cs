using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static Vector2 SpawnPos;

    [SerializeField] private CinemachineCamera _cinemachineCamera;

    [SerializeField] private GameObject _deathUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (SelectionManager.Instance == null)
        {
            return;
        }

        GameObject character = Instantiate(SelectionManager.Instance.CharacterSelectionDataSO.Prefab, SpawnPos, Quaternion.identity);
        _cinemachineCamera.Follow = character.transform;

        character.GetComponent<Health>().OnDie.AddListener(OnDie);

        Instantiate(SelectionManager.Instance.WeaponSelectionDataSO.Prefab, character.transform);

        if (SelectionManager.Instance.PetSelectionDataSO != null)
        {
            GameObject pet = Instantiate(SelectionManager.Instance.PetSelectionDataSO.Prefab, SpawnPos + Vector2.right, Quaternion.identity);
            pet.GetComponent<Pet>().SetPlayerRb(character.GetComponent<Rigidbody2D>());
        }
    }

    private void OnDie()
    {
        StoreManager.Instance.Coins = Mathf.Max(0, StoreManager.Instance.Coins - 10);
        StoreManager.Instance.Gems = Mathf.Max(0, StoreManager.Instance.Gems - 10);
        _deathUI.SetActive(true);
        _deathUI.transform.DOScale(1, 1).SetEase(Ease.OutBounce);
    }

    public void MainMenu()
    {
        SceneTransitioner.Instance.TransitionTo("MainMenu");
    }
}
