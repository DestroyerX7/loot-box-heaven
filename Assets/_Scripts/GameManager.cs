using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static Vector2 SpawnPos;

    [SerializeField] private CinemachineCamera _cinemachineCamera;

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
        Instantiate(SelectionManager.Instance.WeaponSelectionDataSO.Prefab, character.transform);

        if (SelectionManager.Instance.PetSelectionDataSO != null)
        {
            GameObject pet = Instantiate(SelectionManager.Instance.PetSelectionDataSO.Prefab, SpawnPos + Vector2.right, Quaternion.identity);
            pet.GetComponent<Pet>().SetPlayerRb(character.GetComponent<Rigidbody2D>());
        }
    }
}
