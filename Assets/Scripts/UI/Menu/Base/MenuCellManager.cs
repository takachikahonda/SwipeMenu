
using UnityEngine;

public sealed class MenuCellManager : MonoBehaviour
{
    [SerializeField] RectTransform safeArea;
    [SerializeField] MenuSelecter selecter;

    SwipeMenuCell[] cells;
    bool isAleadyAwake;

    void Awake()
    {
        cells = GetComponentsInChildren<SwipeMenuCell>();

        for(int i = 0; i < cells.Length; i++)
        {
            var id = i + 1;
            cells[i].Initialize(id, this);
        }

        selecter.OnSelectedMenu += OnSelectedMenu;
        selecter.OnSwipeMoving += OnSwipeMoving;
    }

    void OnSelectedMenu(int id)
    {
        var isAwake = false;
        if(!isAleadyAwake)
        {
            isAleadyAwake = true;
            isAwake = true;
        }

        Select(id, isAwake);
    }

    void OnSwipeMoving(float moveWidth)
    {
        foreach (var cell in cells)
        {
            cell.Move(cell.CurrentBasePosX - moveWidth);
        }
    }

    void Select(int id, bool isAwake = false)
    {
        var duration = !isAwake ? selecter.Duration : 0;
        var easing = selecter.Easing;

        foreach (var cell in cells)
        {
            var difference = cell.ID - id;

            if (difference == 0)
            {
                cell.OnSelected();
            }
            else
            {
                cell.OnUnselected();
            }

            cell.SetPosition(difference * safeArea.rect.width, duration, easing);
        }
    }

    void Reset()
    {
        safeArea = GetComponentInParent<RectTransform>();
        selecter = GetComponent<MenuSelecter>();
    }
}
