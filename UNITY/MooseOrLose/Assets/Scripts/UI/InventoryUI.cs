using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : UI, IPointerEnterHandler
{
    private List<HorizontalLayoutGroup> _list;
    public static event Action OnExpand;

    private void Awake()
    {
        _list = new List<HorizontalLayoutGroup>(GetComponentsInChildren<HorizontalLayoutGroup>());
    }

    private void Start()
    {
        RuleManager.OnExpand += Shrink;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Expand();
        Debug.Log("Inventory expand");
        OnExpand?.Invoke();
    }

    protected override void Shrink()
    {
        base.Shrink();
        foreach (var hGroup in _list)
        {
            hGroup.gameObject.SetActive(false);
        }
    }
    protected override void Expand()
    {
        base.Expand();
        foreach (var hGroup in _list)
        {
            hGroup.gameObject.SetActive(true);
        }
    }
}
