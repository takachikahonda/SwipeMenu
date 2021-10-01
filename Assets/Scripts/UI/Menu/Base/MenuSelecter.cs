
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public sealed class MenuSelecter : MonoBehaviour
{
    /// <summary>
    /// メニューが選択された時
    /// </summary>
    public event Action<int> OnSelectedMenu = delegate { };
    /// <summary>
    /// スワイプで移動している時
    /// </summary>
    public event Action<float> OnSwipeMoving = delegate { };

    /// <summary>
    /// 最初に選択されているCellのID
    /// </summary>
    public int StartSelectCellID => startSelectCellID;

    /// <summary>
    /// アニメーション速度
    /// </summary>
    public float Duration => duration;
    /// <summary>
    /// アニメーション速度
    /// </summary>
    public Ease Easing => easing;

    /// <summary>
    /// スワイプ可能か？
    /// </summary>
    public bool Swipeable => swipeable;

    [SerializeField] RectTransform fotterRtfm;
    [SerializeField] ScrollRect[] scrollRects;

    [SerializeField] bool useSwipeMove = true; 
    [SerializeField] int startSelectCellID = 1; 
    [SerializeField] float selectedWidth = 350f;
    [SerializeField] float duration = 0.45f;
    [SerializeField] Ease easing = Ease.InOutQuad;
    [SerializeField] float swipeStartWidth = 100;
    float areaWidth => fotterRtfm.rect.width;
    float unselectedWidth => (areaWidth - selectedWidth) / (triggers.Length - 1);

    FeatureImage featureImage;
    SwipeMenuTrigger[] triggers;

    int currentID;
    bool limitterXMove;
    Vector2 startPos;
    bool swipeable;

    void OnEnable()
    {
        IT_Gesture.onTouchDownPosE += OnSwipeStart;
        IT_Gesture.onMouse1DownE += OnSwipeStart;

        IT_Gesture.onTouchPosE += OnSwiping;
        IT_Gesture.onMouse1E += OnSwiping;

        IT_Gesture.onTouchUpPosE += OnSwipeEnd;
        IT_Gesture.onMouse1UpE += OnSwipeEnd;
    }

    void Awake()
    {
        featureImage = fotterRtfm.GetComponentInChildren<FeatureImage>();
        triggers = fotterRtfm.GetComponentsInChildren<SwipeMenuTrigger>();
        for (int i = 0; i < triggers.Length; i++)
        {
            var id = i + 1;
            triggers[i].Initialize(id, this);
        }

        featureImage.Initialized(this);
    }

    void Start()
    {
        Select(startSelectCellID, true);
    }

    /// <summary>
    /// 特定のCellを選択する
    /// </summary>
    public void Select(int id, bool isAwake = false)
    {
        MoveTriggers(id, isAwake);

        foreach (var scrollRect in scrollRects)
        {
            scrollRect.verticalNormalizedPosition = 1;
        }

        OnSelectedMenu?.Invoke(id);
    }

    void MoveTriggers(int id, bool isAwake = false)
    {
        var posX = 0f;
        currentID = id;

        foreach (var trigger in triggers)
        {
            if (trigger.ID == id)
            {
                trigger.SetWidthAndPosition(selectedWidth, posX, !isAwake);
                featureImage.SetWidthAndPosition(selectedWidth, posX, !isAwake);
                trigger.OnSelected(!isAwake);
                posX += selectedWidth;
            }
            else
            {
                trigger.SetWidthAndPosition(unselectedWidth, posX, !isAwake);
                trigger.OnUnselected(!isAwake);
                posX += unselectedWidth;
            }
        }
    }

    /// <summary>
    /// スワイプの可否を設定する
    /// </summary>
    public void SetSwipeable(bool enable)
    {
        swipeable = enable;
    }

    void OnSwipeStart(Vector2 startPos)
    {
        if(!useSwipeMove)
        {
            return;
        }

        this.startPos = startPos;
    }

    void OnSwiping(Vector2 currentPos)
    {
        if (!useSwipeMove)
        {
            return;
        }

        var distanceX = startPos.x - currentPos.x;
        var distanceY = startPos.y - currentPos.y;

        if (Mathf.Abs(distanceY) >= swipeStartWidth)
        {
            limitterXMove = true;
        }

        if (Mathf.Abs(distanceX) >= swipeStartWidth
            && !limitterXMove)
        {
            var moveWidth = distanceX > 0 ? Mathf.Abs(distanceX) - swipeStartWidth : (Mathf.Abs(distanceX) - swipeStartWidth) * -1;
            featureImage.Move(featureImage.CurrentBasePosX + (moveWidth / 5));

            foreach(var scrollRect in scrollRects)
            {
                scrollRect.vertical = false;
            }

            OnSwipeMoving?.Invoke(moveWidth);
        }
    }

    void OnSwipeEnd(Vector2 endPos)
    {
        if (!useSwipeMove)
        {
            return;
        }

        var addID = 0;
        var distanceX = startPos.x - endPos.x;
        var moveRight = distanceX > 0;

        if (Mathf.Abs(distanceX) >= swipeStartWidth
            && !limitterXMove)
        {
            if (moveRight)
            {
                if (currentID < triggers.Length)
                {
                    addID = 1;
                }
            }
            else
            {
                if (currentID > 1)
                {
                    addID = -1;
                }
            }
            Select(currentID + addID);

            foreach (var scrollRect in scrollRects)
            {
                scrollRect.vertical = true;
            }
        }
        else
        {
            MoveTriggers(currentID);
        }

        limitterXMove = false;
    }

#if UNITY_EDITOR

    void Reset()
    {
        FindScrollRect();
    }

    void FindScrollRect()
    {
        scrollRects = FindObjectsOfType<ScrollRect>();
    }

#endif
}
