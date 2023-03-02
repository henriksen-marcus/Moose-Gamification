using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class HuntingGoals : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentHuntingSeason;
    [SerializeField] private TextMeshProUGUI expectedMales;
    [SerializeField] private TextMeshProUGUI expectedFemales;

    public static HuntingGoals Instance;
    
    public List<MonthButton> monthButtons;
    public float ratioGoal = 0.5f;
    public float averageAgeGoal = 1;
    public bool showGoals = true;
    public float squareKmGoal = 3;
    
    private int _minPop;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        monthButtons = new(GetComponentsInChildren<MonthButton>());
    }

    private void Start()
    {
        gameObject.SetActive(false);
        MonthButton.OnMonthButtonChanged += UpdateGoalScreen;
    }

    public void UpdateGoalScreen()
    {
        _minPop = RuleManager.Instance.MoosePopMin;
        ElgManager instance = ElgManager.instance;
        int mooseToShoot = instance.elg_population - _minPop;
        UpdateExpectedMales((mooseToShoot * instance.GetMaleRatio()).ToString("F0"));
        UpdateExpectedFemales((mooseToShoot * (1 - instance.GetMaleRatio())).ToString("F0"));
        currentHuntingSeason.text = "The current season will last from " + (RuleManager.Month)RuleManager.Instance.huntingSeasonRange.Min() + " through " + (RuleManager.Month)RuleManager.Instance.huntingSeasonRange.Max() + ".";
    }
    public void SetShow(bool show)
    {
        showGoals = show;
    }
    public void EndGoalSetting()
    {
        TimeManager.instance.SetGamePaused(false);
        // InfoUI.Instance.gameObject.SetActive(true);
        InventoryUI.Instance.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    
    public void UpdateRatioGoal(string inString)
    {
        float.TryParse(inString, out ratioGoal);
        InfoUI.Instance.UpdateRatioGoal(ratioGoal);
    }
    public void UpdateAgeGoal(string inString)
    {
        float.TryParse(inString, out averageAgeGoal);
        InfoUI.Instance.UpdateAgeGoal(averageAgeGoal);
    }
    public void UpdateSquareKmGoal(string inString)
    {
        float.TryParse(inString, out squareKmGoal);
        InfoUI.Instance.UpdateSquareKmGoal(squareKmGoal);
    }
    private void UpdateExpectedMales(string inString)
    {
        expectedMales.text = inString;
    }
    private void UpdateExpectedFemales(string inString)
    {
        expectedFemales.text = inString;
    }
}
