using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingChest : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private RectTransform _rectTransform;

    [SerializeField] private float _maxHoverHeight = 25;
    [SerializeField] private float _hoverDuration = 60;

    private Tween _lastShakeTween;

    private Tween _hoverTween;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _hoverTween = _rectTransform.DOAnchorPosY(_rectTransform.anchoredPosition.y + _maxHoverHeight, _hoverDuration).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnEnable()
    {
        _hoverTween.Play();
    }

    private void OnDisable()
    {
        _hoverTween.Pause();
        _lastShakeTween?.Kill();
        _rectTransform.localRotation = Quaternion.identity;
    }

    private void OnDestroy()
    {
        transform.DOKill();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _lastShakeTween?.Kill();
        _rectTransform.localRotation = Quaternion.identity;
        _lastShakeTween = _rectTransform.DOShakeRotation(0.5f, new Vector3(0, 0, 5), vibrato: 15, randomnessMode: ShakeRandomnessMode.Harmonic);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _rectTransform.DOScale(1.01f, 0.25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rectTransform.DOScale(1, 0.25f);
    }
}
