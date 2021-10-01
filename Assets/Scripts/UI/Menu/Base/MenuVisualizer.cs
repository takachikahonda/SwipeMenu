
using UnityEngine;

public sealed class MenuVisualizer : MonoBehaviour
{
    [SerializeField] bool startShow = true;

    [SerializeField] CanvasGroup canvasGroup;

    void Start()
    {
        if(startShow)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
