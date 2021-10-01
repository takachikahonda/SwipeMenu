
using UnityEngine;
using DG.Tweening;

public sealed class FeatureImage : MonoBehaviour
{
    /// <summary>
    /// 現在のX座標
    /// </summary>
    public float CurrentBasePosX => currentBasePosX;

    [SerializeField] RectTransform rectTransform;

    MenuSelecter selecter;
    Tween sizeTween;
    Tween posTween;

    float currentBasePosX;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialized(MenuSelecter selecter)
    {
        this.selecter = selecter;
    }

    /// <summary>
    /// Cellの長さと位置を設定する
    /// </summary>
    public void SetWidthAndPosition(float width, float posX, bool useAnimation = true)
    {
        sizeTween.Kill();
        posTween.Kill();

        currentBasePosX = posX;

        sizeTween = DOVirtual.Float(rectTransform.rect.width, width, useAnimation ? selecter.Duration : 0, value =>
        {
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);
        }).SetEase(selecter.Easing);
        posTween = DOVirtual.Float(rectTransform.anchoredPosition.x, posX, useAnimation ? selecter.Duration : 0, value =>
        {
            rectTransform.anchoredPosition = new Vector2(value, rectTransform.anchoredPosition.y);
        }).SetEase(selecter.Easing);
    }

    /// <summary>
    /// 指定されたX座標に移動する
    /// </summary>
    public void Move(float posX)
    {
        rectTransform.anchoredPosition = new Vector2(posX, rectTransform.anchoredPosition.y);
    }

    void Reset()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}
