using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverTipManager : MonoBehaviour
{
    [SerializeField] private RectTransform tooltipWindow;
    [SerializeField] private TextMeshProUGUI tipText;
    
    public Action<string, Vector2> OnMouseHover;
    public Action OnMouseLoseFocus;

    public static HoverTipManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        HideTip();
    }

    private void OnEnable()
    {
        OnMouseHover += ShowTip;
        OnMouseLoseFocus += HideTip;
    }

    private void OnDisable()
    {
        OnMouseHover -= ShowTip;
        OnMouseLoseFocus -= HideTip;
    }

    private void ShowTip(string tip, Vector2 mousePos)
    {
        tipText.text = tip;
        tooltipWindow.sizeDelta = new Vector2(tipText.preferredWidth > 200 ? 200 : tipText.preferredWidth + 2, tipText.preferredHeight + 1);
        
        tooltipWindow.gameObject.SetActive(true);
        tooltipWindow.transform.position = new Vector2(mousePos.x + tooltipWindow.sizeDelta.x / 2, mousePos.y);
    }

    private void HideTip()
    {
        tipText.text = default;
        tooltipWindow.gameObject.SetActive(false);
    }
}
