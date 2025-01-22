using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    public static SceneTransitioner Instance { get; private set; }

    [SerializeField] private float _transitionDuration = 1;

    private Animator _animator;

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
        _animator = GetComponent<Animator>();
    }

    public void TransitionTo(string sceneName)
    {
        StartCoroutine(TransitionToCoroutine(sceneName));
    }

    private IEnumerator TransitionToCoroutine(string sceneName)
    {
        GameManager.Instance.Pause();
        _animator.SetTrigger("TransitionOut");

        yield return new WaitForSeconds(_transitionDuration);

        SceneManager.LoadScene(sceneName);
    }
}
