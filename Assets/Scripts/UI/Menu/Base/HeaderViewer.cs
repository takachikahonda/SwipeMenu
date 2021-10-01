
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public sealed class HeaderViewer : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] MenuSelecter selecter;

    [SerializeField, Space(10)] List<int> hideMenuIDs;

    void Awake()
    {
        selecter.OnSelectedMenu += OnSelectedMenu;
    }

    void OnSelectedMenu(int id)
    {
        if(hideMenuIDs.Contains(id))
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    void Show(bool useAnimation = true)
    {
        canvasGroup.DOFade(1, useAnimation ? selecter.Duration : 0).SetEase(selecter.Easing);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    void Hide(bool useAnimation = true)
    {
        canvasGroup.DOFade(0, useAnimation ? selecter.Duration : 0).SetEase(selecter.Easing);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
