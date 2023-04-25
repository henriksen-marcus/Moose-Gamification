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

    public ArrayList Rules = new ArrayList();
    private int startDay;

    public Dictionary<string, string> RuleTooltips;

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
        TimeManager.Instance.OnNewMonth += HuntingSeasonGoals;
        TimeManager.Instance.OnNewMonth += HuntingSeasonReview;

        HuntingSeasonGoals();

        InitiateRules();
        
        RuleTooltips.Add("Average bull age", "Desired average age for the bull moose population.");
        RuleTooltips.Add("Ratio of bulls to total population", "Determines the desired percent of the moose population that should be male. (1 = 100%, 0.5 = 50%...)");
        RuleTooltips.Add("Moose to shot per km^2", "How many moose should be shot per square km every hunting season?");
        RuleTooltips.Add("Minimum antler tags to shoot mose", "Don't shoot moose that have fewer antler tags than this.");
        RuleTooltips.Add("Calves to leave alive", "When shooting calves, always leave at least this many calves alive. The mother is only ever shot if she has 0 calves.");
        RuleTooltips.Add("Weekly moose shooting limit", "The max number of moose any single hunter can shoot in a single week.");
        RuleTooltips.Add("Minimum moose population", "Don't hunt any more moose if there are this amount of moose left, or fewer.");
        RuleTooltips.Add("Maximum wolf population", "Start hunting wolves if the amount of wolves on the map goes above this limit.");
    }

    private void HuntingSeasonGoals()
    {
        if (HuntingSeason() && !_lastMonthWasHuntingSeason)
        {
            OnHuntingSeasonStart?.Invoke();
            if (HuntingGoals.Instance.showGoals)
            {
                gameObject.SetActive(true);
                TimeManager.Instance.SetGamePaused(true);
                _lastMonthWasHuntingSeason = true;
                HuntingGoals.Instance.gameObject.SetActive(true);
                HuntingGoals.Instance.UpdateGoalScreen();
                InventoryUI.Instance.gameObject.SetActive(false);
                // InfoUI.Instance.gameObject.SetActive(false);
            }
        }
    }
    private void InitiateRules()
    {
        //Debug.Log("Initiating Rules");
        startDay = TimeManager.Instance.GetDay();
        // AverageMaleAge
        Rules.Add(new Rule<float> { 
            Name = "Average Male Age", 
            Intervals = new List<Interval<float>>()
            {               
                new Interval<float> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = HuntingGoals.Instance.averageAgeGoal}                
            }                    
        });
        Rule<float> ruleF = (Rule<float>)Rules[0];
        //Debug.Log(ruleF.Name + " Changed to " + ruleF.Intervals[ruleF.Intervals.Count - 1].Value.ToString() + " on day: " + ruleF.Intervals[ruleF.Intervals.Count - 1].StartDay);
        // Male To Population Ratio
        Rules.Add(new Rule<float>
        {
            Name = "Male Ratio",
            Intervals = new List<Interval<float>>()
            {
                new Interval<float> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = HuntingGoals.Instance.ratioGoal}
            }
        });
        ruleF = (Rule<float>)Rules[1];
        //Debug.Log(ruleF.Name + " Changed to " + ruleF.Intervals[ruleF.Intervals.Count - 1].Value.ToString() + " on day: " + ruleF.Intervals[ruleF.Intervals.Count - 1].StartDay);
        // Moose to Shoot per km^2
        Rules.Add(new Rule<float>
        {
            Name = "Shooting Goal",
            Intervals = new List<Interval<float>>()
            {
                new Interval<float> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = HuntingGoals.Instance.squareKmGoal}
            }
        });
        ruleF = (Rule<float>)Rules[2];
        //Debug.Log(ruleF.Name + " Changed to " + ruleF.Intervals[ruleF.Intervals.Count - 1].Value.ToString() + " on day: " + ruleF.Intervals[ruleF.Intervals.Count - 1].StartDay);
        // Antler Limit
        Rules.Add(new Rule<int>
        {
            Name = "Antler Limit",
            Intervals = new List<Interval<int>>()
            {
                new Interval<int> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = _antlerPointsLimit}
            }
        });
        Rule<int> ruleI = (Rule<int>)Rules[3];
        //Debug.Log(ruleI.Name + " Changed to " + ruleI.Intervals[ruleI.Intervals.Count - 1].Value.ToString() + " on day: " + ruleI.Intervals[ruleI.Intervals.Count - 1].StartDay);
        // Calves To Leave Alive
        Rules.Add(new Rule<int>
        {
            Name = "Calf Limit",
            Intervals = new List<Interval<int>>()
            {
                new Interval<int> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = _calfLimit}
            }
        });
        ruleI = (Rule<int>)Rules[4];
        //Debug.Log(ruleI.Name + " Changed to " + ruleI.Intervals[ruleI.Intervals.Count - 1].Value.ToString() + " on day: " + ruleI.Intervals[ruleI.Intervals.Count - 1].StartDay);
        // WeeklyLimit
        Rules.Add(new Rule<int>
        {
            Name = "Shoot Limit",
            Intervals = new List<Interval<int>>()
            {
                new Interval<int> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = _weeklyMooseLimit}
            }
        });
        ruleI = (Rule<int>)Rules[5];
        //Debug.Log(ruleI.Name + " Changed to " + ruleI.Intervals[ruleI.Intervals.Count - 1].Value.ToString() + " on day: " + ruleI.Intervals[ruleI.Intervals.Count - 1].StartDay);
        // Min Population Limit
        Rules.Add(new Rule<int>
        {
            Name = "Population Limit",
            Intervals = new List<Interval<int>>()
            {
                new Interval<int> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = MoosePopMin}
            }
        });
        ruleI = (Rule<int>)Rules[6];
        //Debug.Log(ruleI.Name + " Changed to " + ruleI.Intervals[ruleI.Intervals.Count - 1].Value.ToString() + " on day: " + ruleI.Intervals[ruleI.Intervals.Count - 1].StartDay);
        // WolfLimit
        Rules.Add(new Rule<int>
        {
            Name = "Wolf Limit",
            Intervals = new List<Interval<int>>()
            {
                new Interval<int> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = _wolfLimit}
            }
        });
        ruleI = (Rule<int>)Rules[7];
        //Debug.Log(ruleI.Name + " Changed to " + ruleI.Intervals[ruleI.Intervals.Count - 1].Value.ToString() + " on day: " + ruleI.Intervals[ruleI.Intervals.Count - 1].StartDay);


    }
    public void ToggleHuntingSeason()
    {
        if (HuntingGoals.Instance.gameObject.activeSelf)
        {
            HuntingGoals.Instance.gameObject.SetActive(false);
            TimeManager.Instance.SetGamePaused(false);
            CheckForRuleUpdates();
        }
        else
        {
            HuntingGoals.Instance.gameObject.SetActive(true);
            HuntingGoals.Instance.UpdateGoalScreen();
            TimeManager.Instance.SetGamePaused(true);
        }
    }

    public void CheckForRuleUpdates()
    {
        foreach (var rule in Rules)
        {
            Rule<float> ruleF = null;
            Rule<int> ruleI = null;
            try { 
                ruleF = (Rule<float>)rule;
            } 
            catch {                  
            }
            try
            {
                ruleI = (Rule<int>)rule;
            }
            catch {                
            }
            if (ruleI != null)
            {
                switch (ruleI.Name)
                {
                    case "Antler Limit":
                        if (ruleI.Intervals[ruleI.Intervals.Count - 1].Value != _antlerPointsLimit)
                        {
                            ruleI.Intervals.Add(new Interval<int> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = _antlerPointsLimit }); 
                            //Debug.Log(ruleI.Name + " Changed to " + ruleI.Intervals[ruleI.Intervals.Count - 1].Value.ToString() + " on day: " + ruleI.Intervals[ruleI.Intervals.Count - 1].StartDay);
                        }
                        break;
                    case "Calf Limit":
                        if (ruleI.Intervals[ruleI.Intervals.Count - 1].Value != _calfLimit)
                        {
                            ruleI.Intervals.Add(new Interval<int> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = _calfLimit });
                            //Debug.Log(ruleI.Name + " Changed to " + ruleI.Intervals[ruleI.Intervals.Count - 1].Value.ToString() + " on day: " + ruleI.Intervals[ruleI.Intervals.Count - 1].StartDay);
                        }
                        break;
                    case "Shoot Limit":
                        if (ruleI.Intervals[ruleI.Intervals.Count - 1].Value != _weeklyMooseLimit)
                        {
                            ruleI.Intervals.Add(new Interval<int> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = _weeklyMooseLimit });
                            //Debug.Log(ruleI.Name + " Changed to " + ruleI.Intervals[ruleI.Intervals.Count - 1].Value.ToString() + " on day: " + ruleI.Intervals[ruleI.Intervals.Count - 1].StartDay);
                        }
                        break;
                    case "Population Limit":
                        if (ruleI.Intervals[ruleI.Intervals.Count - 1].Value != MoosePopMin)
                        {
                            ruleI.Intervals.Add(new Interval<int> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = MoosePopMin });
                            //Debug.Log(ruleI.Name + " Changed from to " + ruleI.Intervals[ruleI.Intervals.Count - 1].Value.ToString() + " on day: " + ruleI.Intervals[ruleI.Intervals.Count - 1].StartDay);
                        }
                        break;
                    case "Wolf Limit":
                        if (ruleI.Intervals[ruleI.Intervals.Count - 1].Value != _wolfLimit)
                        {
                            ruleI.Intervals.Add(new Interval<int> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = _wolfLimit });
                            //Debug.Log(ruleI.Name + " Changed from to " + ruleI.Intervals[ruleI.Intervals.Count - 1].Value.ToString() + " on day: " + ruleI.Intervals[ruleI.Intervals.Count - 1].StartDay);
                        }
                        break;
                    default:
                        break;
                }
                continue;
            }
            if (ruleF != null)
            {
                switch (ruleF.Name)
                {
                    case "Average Male Age":
                        if (ruleF.Intervals[ruleF.Intervals.Count - 1].Value != HuntingGoals.Instance.averageAgeGoal)
                        {
                            ruleF.Intervals.Add(new Interval<float> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = HuntingGoals.Instance.averageAgeGoal });
                            //Debug.Log(ruleF.Name + " Changed from to " + ruleF.Intervals[ruleF.Intervals.Count - 1].Value.ToString() + " on day: " + ruleF.Intervals[ruleF.Intervals.Count - 1].StartDay);
                        }
                        break;
                    case "Male Ratio":
                        if (ruleF.Intervals[ruleF.Intervals.Count - 1].Value != HuntingGoals.Instance.ratioGoal)
                        {
                            ruleF.Intervals.Add(new Interval<float> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = HuntingGoals.Instance.ratioGoal });
                            //Debug.Log(ruleF.Name + " Changed from to " + ruleF.Intervals[ruleF.Intervals.Count - 1].Value.ToString() + " on day: " + ruleF.Intervals[ruleF.Intervals.Count - 1].StartDay);
                        }
                        break;
                    case "Shooting Goal":
                        if (ruleF.Intervals[ruleF.Intervals.Count - 1].Value != HuntingGoals.Instance.squareKmGoal)
                        {
                            ruleF.Intervals.Add(new Interval<float> { StartDay = TimeManager.Instance.GetDay() + (TimeManager.Instance.GetYear() * 360) - startDay, Value = HuntingGoals.Instance.squareKmGoal });
                            //Debug.Log(ruleF.Name + " Changed from to " + ruleF.Intervals[ruleF.Intervals.Count - 1].Value.ToString() + " on day: " + ruleF.Intervals[ruleF.Intervals.Count - 1].StartDay);
                        }
                        break;
                    default:
                        break;
                }
                continue;
            }
            
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
                TimeManager.Instance.SetGamePaused(true);
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
    public bool CanShootFemale(int children, int shotThisWeek) //always shoot cow last? (children == )
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
        return huntingSeasonRange.Contains(TimeManager.Instance.GetMonth());
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
