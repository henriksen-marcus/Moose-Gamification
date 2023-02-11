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
    [SerializeField] private GameObject seasonGoals;
    [SerializeField] private List<int> huntingSeasonRange;

    private int _antlerPointsLimit = 4;
    private int _dailyMooseLimit = 5;
    public int moosePopMin = 60;
    private int _calfLimit = 1;
    private int _wolfLimit = 30;
    // private bool _huntingSeason = false;
    private bool _lastMonthWasHuntingSeason = false;
    private List<MonthButton> _monthButtons;
    private List<HorizontalLayoutGroup> _list;


    public static event Action OnExpand;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _monthButtons = new(GetComponentsInChildren<MonthButton>());
        _list = new(GetComponentsInChildren<HorizontalLayoutGroup>());
    }

    private void Start()
    {
        Shrink();
        SetHuntingSeasonRange();
        Debug.Log("Rule shrink");
        InventoryUI.OnExpand += Shrink;
        MonthButton.OnMonthButtonChanged += SetHuntingSeasonRange;
        TimeManager.instance.OnNewMonth += HuntingSeasonGoals;
        TimeManager.instance.OnNewMonth += HuntingSeasonReview;
    }

    private void HuntingSeasonGoals()
    {
        if (HuntingSeason() && !_lastMonthWasHuntingSeason)
        {
            TimeManager.instance.SetGamePaused(true);
            _lastMonthWasHuntingSeason = true;
            seasonGoals.SetActive(true);
        }
    }
    private void HuntingSeasonReview()
    {
        if (!HuntingSeason() && _lastMonthWasHuntingSeason)
        {
            //TODO review screen

            _lastMonthWasHuntingSeason = false;
        }
    }
    
    //Get values - other classes refer to these
    public bool CanShootMale(int horns, int shotToday)
    {
        return horns >= _antlerPointsLimit && shotToday < _dailyMooseLimit && ElgManager.instance.elg_population > moosePopMin && HuntingSeason();
    }
    public bool CanShootFemale(int children, int shotToday) //always shoot cow last? (children == 0)
    {
        return shotToday < _dailyMooseLimit && ElgManager.instance.elg_population > moosePopMin && children == 0 && HuntingSeason();
    }
    public bool CanShootChild(int children, int shotToday)
    {
        return children > _calfLimit && ElgManager.instance.elg_population > moosePopMin && shotToday < _dailyMooseLimit && HuntingSeason();
    }
    public bool CanShootWolf()
    {
        return UlvManager.instance.ulv_population > _wolfLimit;
    }
    public bool HuntingSeason()
    {
        return huntingSeasonRange.Contains(TimeManager.instance.GetMonth());
    }
    
    
    //UI - setting values
    public void SetAntlerPointLimit(string inLimit)
    {
        int.TryParse(inLimit, out _antlerPointsLimit);
    }
    public void SetCalfLimit(string inLimit)
    {
        int.TryParse(inLimit, out _calfLimit);
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
    public void SetWolfPopLimit(string inLimit)
    {
        int.TryParse(inLimit, out _wolfLimit);
    }
    public void SetMoosePopMin(string inLimit)
    {
        int.TryParse(inLimit, out moosePopMin);
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
