
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public sealed class SwipeMenuTrigger : MonoBehaviour
{
    /// <summary>
    /// 識別ID
    /// </summary>
    public int ID => id;
    int id;

    [SerializeField] RectTransform rectTransform;
    [SerializeField] EventTrigger trigger;
    [SerializeField] RectTransform iconRtfm;
    [SerializeField] Image[] arrows;
    [SerializeField] Text text;

    MenuSelecter selecter;
    Tween sizeTween;
    Tween posTween;

    float iconScaleSize = 1.25f;
    float contentsFadeSpeed = 2.0f;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(int id, MenuSelecter controller)
    {
        this.id = id;
        this.selecter = controller;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((data) => { OnTriggerSelected(); });
        trigger.triggers.Add(entry);
    }

    void OnTriggerSelected() => selecter.Select(ID);

    /// <summary>
    /// 選択された状態になった時
    /// </summary>
    public void OnSelected(bool useAnimation = true)
    {
        iconRtfm.DOAnchorPosY(50, selecter.Duration / contentsFadeSpeed).SetEase(selecter.Easing);
        iconRtfm.DOScale(iconScaleSize, selecter.Duration / contentsFadeSpeed).SetEase(selecter.Easing);
        for (int i = 0; i < arrows.Length; i++) arrows[i].DOFade(1, selecter.Duration / contentsFadeSpeed).SetEase(selecter.Easing);
        text.DOFade(1, selecter.Duration / contentsFadeSpeed).SetEase(selecter.Easing);
    }

    /// <summary>
    /// 選択されていない状態になった時
    /// </summary>
    public void OnUnselected(bool useAnimation = true)
    {
        iconRtfm.DOAnchorPosY(0, selecter.Duration / contentsFadeSpeed).SetEase(selecter.Easing);
        iconRtfm.DOScale(1, selecter.Duration / contentsFadeSpeed).SetEase(selecter.Easing);
        for (int i = 0; i < arrows.Length; i++) arrows[i].DOFade(0, selecter.Duration / contentsFadeSpeed).SetEase(selecter.Easing);
        text.DOFade(0, selecter.Duration / contentsFadeSpeed).SetEase(selecter.Easing);
    }

    /// <summary>
    /// Cellの長さと位置を設定する
    /// </summary>
    public void SetWidthAndPosition(float width, float posX, bool useAnimation = true)
    {
        sizeTween.Kill();
        posTween.Kill();

        sizeTween = DOVirtual.Float(rectTransform.rect.width, width, useAnimation ? selecter.Duration : 0, value =>
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
        }).SetEase(selecter.Easing);
        posTween = DOVirtual.Float(rectTransform.anchoredPosition.x, posX, useAnimation ? selecter.Duration : 0, value =>
        {
            rectTransform.anchoredPosition = new Vector2(value, rectTransform.anchoredPosition.y);
        }).SetEase(selecter.Easing);
    }

    void Reset()
    {
        rectTransform = GetComponent<RectTransform>();
        trigger = GetComponent<EventTrigger>();
    }
}
