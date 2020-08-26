using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    private static BaseUI instance { get; set; }
    private Base playerBase;

    private void Awake()
    {
        instance = this;
        playerBase = gameObject.GetComponentInParent<PlayerBase>();
        RefreshRangeVisual();
        //Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void RefreshRangeVisual()
    {
        transform.Find("BuildRange").localScale = Vector3.one * playerBase.getBuildRange() * 2f;
    }
}
