using UnityEngine;

public class SceneTransitionArea : MonoBehaviour, IInteractable
{
    [SerializeField] private string _sceneName;
    [SerializeField] private Vector2 _spawnPos;

    public bool CanInteract { get; private set; }

    public string Message { get; private set; }

    private void Start()
    {
        Message = _sceneName;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            CanInteract = false;
        }
    }

    public void Interact()
    {
        GameManager.SpawnPos = _spawnPos;
        SceneTransitioner.Instance.TransitionTo(_sceneName);
    }
}
