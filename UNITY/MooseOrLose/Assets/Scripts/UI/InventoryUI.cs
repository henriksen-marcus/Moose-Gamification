using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : UI, IPointerEnterHandler
{
    public static InventoryUI Instance;
    
    private List<HorizontalLayoutGroup> _list;
    public event Action OnExpand;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        _list = new(GetComponentsInChildren<HorizontalLayoutGroup>());
    }


    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Expand();
        Debug.Log("Inventory expand");
        OnExpand?.Invoke();
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
