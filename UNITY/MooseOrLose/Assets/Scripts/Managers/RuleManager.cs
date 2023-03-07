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
    public List<int> huntingSeasonRange;

    [NonSerialized] public int MoosePopMin = 60;
    private int _antlerPointsLimit = 4;
    private int _weeklyMooseLimit = 5;
    // moose per square kilometer
    private int _calfLimit = 1;
    private int _wolfLimit = 30;
    // private bool _huntingSeason = false;
    private bool _lastMonthWasHuntingSeason = false;
    private List<HorizontalLayoutGroup> _list;

    public event Action OnHuntingSeasonStart;
    public event Action OnHuntingSeasonEnd;

    // public event Action OnExpand;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _list = new(GetComponentsInChildren<HorizontalLayoutGroup>());
    }

    private void Start()
    {
        SetHuntingSeasonRange();
        MonthButton.OnMonthButtonChanged += SetHuntingSeasonRange;
        TimeManager.instance.OnNewMonth += HuntingSeasonGoals;
        TimeManager.instance.OnNewMonth += HuntingSeasonReview;

        HuntingSeasonGoals();
    }

    private void HuntingSeasonGoals()
    {
        if (HuntingSeason() && !_lastMonthWasHuntingSeason)
        {
            OnHuntingSeasonStart?.Invoke();
            if (HuntingGoals.Instance.showGoals)
            {
                gameObject.SetActive(true);
                TimeManager.instance.SetGamePaused(true);
                _lastMonthWasHuntingSeason = true;
                HuntingGoals.Instance.gameObject.SetActive(true);
                HuntingGoals.Instance.UpdateGoalScreen();
                InventoryUI.Instance.gameObject.SetActive(false);
                // InfoUI.Instance.gameObject.SetActive(false);
            }
        }
    }
    
    public void ToggleHuntingSeason()
    {
        if (HuntingGoals.Instance.gameObject.activeSelf)
        {
            HuntingGoals.Instance.gameObject.SetActive(false);
            TimeManager.instance.SetGamePaused(false);
        }
        else
        {
            HuntingGoals.Instance.gameObject.SetActive(true);
            HuntingGoals.Instance.UpdateGoalScreen();
            TimeManager.instance.SetGamePaused(true);
        }
    }

    private void HuntingSeasonReview()
    {
        // Debug.Log(HuntingSeason() + _lastMonthWasHuntingSeason.ToString() + StatisticsUI.Instance.showStatistics);
        if (!HuntingSeason() && _lastMonthWasHuntingSeason)
        {
            OnHuntingSeasonEnd?.Invoke();
            if (StatisticsUI.Instance.showStatistics)
            {
                StatisticsUI.Instance.gameObject.SetActive(true);
                StatisticsUI.Instance.UpdateGraph();
                TimeManager.instance.SetGamePaused(true);
                _lastMonthWasHuntingSeason = false;
                // Debug.Log("Hunting season review");
            }
        }
    }
    
    //Get values - other classes refer to these
    public bool CanShootMale(int horns, int shotThisWeek)
    {
        return horns >= _antlerPointsLimit && shotThisWeek < _weeklyMooseLimit && ElgManager.instance.elg_population > MoosePopMin && HuntingSeason();
    }
    public bool CanShootFemale(int children, int shotThisWeek) //always shoot cow last? (children == 0)
    {
        return shotThisWeek < _weeklyMooseLimit && ElgManager.instance.elg_population > MoosePopMin && children == 0 && HuntingSeason();
    }
    public bool CanShootChild(int children, int shotToday)
    {
        return children > _calfLimit && ElgManager.instance.elg_population > MoosePopMin && shotToday < _weeklyMooseLimit && HuntingSeason();
    }
    public bool CanShootWolf()
    {
        return UlvManager.instance.ulv_population > _wolfLimit;
    }
    public bool HuntingSeason()
    {
        return huntingSeasonRange.Contains(TimeManager.instance.GetMonth());
    }

    public int SeasonMaleQuota()
    {
        if (HuntingSeason())
        {
            HuntingGoals goalsInstance = HuntingGoals.Instance;
            return (int)(goalsInstance.squareKmGoal * 15 * (1 - goalsInstance.ratioGoal));
        }
        else
        {
            return 0;
        }
    }
    public int SeasonFemaleQuota()
    {
        if (HuntingSeason())
        {
            HuntingGoals goalsInstance = HuntingGoals.Instance;
            return (int)(goalsInstance.squareKmGoal * 15 * goalsInstance.ratioGoal);
        }
        else
        {
            return 0;
        }
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
        int.TryParse(inLimit, out _weeklyMooseLimit);
    }
    private void SetHuntingSeasonRange()
    {
        huntingSeasonRange.Clear();
        foreach (var monthButton in HuntingGoals.Instance.monthButtons)
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
        int.TryParse(inLimit, out MoosePopMin);
    }
    
    //UI functionality
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Expand();
        // Debug.Log("Rule expand");
        // OnExpand?.Invoke();
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
    
    public enum Month
    {
        January = 0,
        February = 1,
        March = 2,
        April = 3,
        May = 4,
        June = 5,
        July = 6,
        August = 7,
        September = 8,
        October = 9,
        November = 10,
        December = 11
    }
}
