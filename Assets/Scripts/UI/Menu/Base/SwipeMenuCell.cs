
using System;
using UnityEngine;
using DG.Tweening;

public abstract class SwipeMenuCell : MonoBehaviour
{
    /// <summary>
    /// 移動開始時
    /// </summary>
    public Action OnMoveStart = delegate { };
    /// <summary>
    /// 移動終了時
    /// </summary>
    public Action OnMoveFinish = delegate { };

    /// <summary>
    /// 識別ID
    /// </summary>
    public int ID => id;
    /// <summary>
    /// 現在のX座標
    /// </summary>
    public float CurrentBasePosX => currentBasePosX;

    [SerializeField] RectTransform rectTransform;
    [SerializeField] CanvasGroup canvasGroup;

    protected MenuCellManager manager;
    Tween posTween;

    int id;
    float currentBasePosX;

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(int id, MenuCellManager manager)
    {
        this.id = id;
        this.manager = manager;

        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// 選択された状態になった時
    /// </summary>
    public virtual void OnSelected()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// 選択されていない状態になった時
    /// </summary>
    public virtual void OnUnselected()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Cellの長さと位置を設定する
    /// </summary>
    public void SetPosition(float posX, float duration, Ease easing)
    {
        OnMoveStart?.Invoke();
        posTween.Kill();

        currentBasePosX = posX;

        posTween = DOVirtual.Float(rectTransform.anchoredPosition.x, posX, duration, value =>
        {
            rectTransform.anchoredPosition = new Vector2(value, rectTransform.anchoredPosition.y);
        }).SetEase(easing).OnComplete(() => OnMoveFinish?.Invoke());
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
        canvasGroup = GetComponentInChildren<CanvasGroup>();
    }
}
