using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _interactAction;

    [SerializeField] private float _interactionRange = 1.5f;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _interactAction = _playerInput.actions["Interact"];
    }

    private void Update()
    {
        IInteractable interactable = Physics2D.OverlapCircle(transform.position, _interactionRange, LayerMask.GetMask("Interactable"))?.GetComponent<IInteractable>();

        if (interactable != null && interactable.CanInteract)
        {
            InteractionManager.Instance.ShowInteractionUI("F", interactable.Message);

            if (_interactAction.triggered || Input.GetKeyDown(KeyCode.F))
            {
                interactable.Interact();
            }
        }
        else
        {
            InteractionManager.Instance.HideInteractionUI();
        }
    }
}

public interface IInteractable
{
    public bool CanInteract { get; }
    public string Message { get; }

    public void Interact();
}
