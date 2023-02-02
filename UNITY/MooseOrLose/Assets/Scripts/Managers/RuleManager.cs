using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuleManager : UI, IPointerEnterHandler
{
    // Singleton
    public static RuleManager Instance;
    
    //moving rules from other managers here?
    [SerializeField] private List<int> _huntingSeasonRange;
    private List<MonthButton> _monthButtons;
    private bool _huntingSeason;
    private List<HorizontalLayoutGroup> _list;


    public static event Action OnExpand;

    private void Awake()
    {
        _monthButtons = new(GetComponentsInChildren<MonthButton>());
        _list = new(GetComponentsInChildren<HorizontalLayoutGroup>());
    }

    private void Start()
    {
        Shrink();
        Debug.Log("Rule shrink");
        InventoryUI.OnExpand += Shrink;
        MonthButton.OnMonthButtonChanged += SetHuntingSeasonRange;
    }

    public bool HuntingSeason()
    {
        return _huntingSeasonRange.Contains(TimeManager.instance.GetMonth());
    }

    private void SetHuntingSeasonRange()
    {
        _huntingSeasonRange.Clear();
        foreach (var monthButton in _monthButtons)
        {
            if(monthButton.HuntingEnabled())
                _huntingSeasonRange.Add(monthButton.Month());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Expand();
        Debug.Log("Rule expand");
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
