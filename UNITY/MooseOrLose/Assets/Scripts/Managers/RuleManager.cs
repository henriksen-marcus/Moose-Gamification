using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuleManager : UI, IPointerEnterHandler
{
    // Singleton
    public static RuleManager Instance;
    
    //moving rules from other managers here?
    [SerializeField] private List<int> huntingSeasonRange;
    
    private int _hornLimit = 4;
    private int _dailyMooseLimit = 5;
    private int _moosePopMin = 60;
    private int _childLimit = 2;
    private int _wolfLimit = 30;
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

    //Get values - other classes refer to these
    public bool CanShootMale(int horns, int shotToday)
    {
        return horns >= _hornLimit && shotToday < _dailyMooseLimit && ElgManager.instance.elg_population > _moosePopMin;
    }
    public bool CanShootFemale(int children)
    {
        return children >= _childLimit && ElgManager.instance.elg_population > _moosePopMin;
    }
    public bool CanShootWolf()
    {
        return true;//wolf manager.pop > _wolfLimit
    }
    public bool HuntingSeason()
    {
        return huntingSeasonRange.Contains(TimeManager.instance.GetMonth());
    }
    
    
    //UI - setting values
    public void SetHornLimit(string inLimit)
    {
        int.TryParse(inLimit, out _hornLimit);
    }
    public void SetChildLimit(string inLimit)
    {
        int.TryParse(inLimit, out _childLimit);
    }
    public void SetDailyMooseLimit(string inLimit)
    {
        int.TryParse(inLimit, out _dailyMooseLimit);
    }
    private void SetHuntingSeasonRange()
    {
        huntingSeasonRange.Clear();
        foreach (var monthButton in _monthButtons)
        {
            if(monthButton.HuntingEnabled())
                huntingSeasonRange.Add(monthButton.Month());
        }
    }

    
    //UI functionality
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
