using UnityEngine;

public class DialogCharacter : MonoBehaviour, IDamageable, IInteractable
{
    [SerializeField] private string[] _dialog;
    [SerializeField] private Sprite _faceSprite;
    [SerializeField] private GameObject _speechBubble;

    private bool _isTalking;

    public bool CanInteract { get => !_isTalking; }
    public string Message { get; } = "Talk";

    private void FixedUpdate()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, 1, LayerMask.GetMask("Player"));

        if (player != null && !_isTalking)
        {
            _speechBubble.SetActive(true);
        }
        else if (player == null)
        {
            _speechBubble.SetActive(false);
            _isTalking = false;
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         _speechBubble.SetActive(true);
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         _speechBubble.SetActive(false);
    //         _isTalking = false;
    //     }
    // }

    public void TakeDamage(float damage)
    {
        DialogManager.Instance.StartDialog(_faceSprite, "Ouch! Why would you try and hurt me? I'm just a friendly NPC. Don't do that again! :(");
        _isTalking = true;
        _speechBubble.SetActive(false);
    }

    public void Interact()
    {
        DialogManager.Instance.StartDialog(_faceSprite, _dialog);
        _isTalking = true;
        _speechBubble.SetActive(false);
    }
}
