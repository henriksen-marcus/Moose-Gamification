using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HuntingGoals : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentHuntingSeason;
    [SerializeField] private TextMeshProUGUI expectedMales;
    [SerializeField] private TextMeshProUGUI expectedFemales;

    public static HuntingGoals Instance;
    
    public float ratioGoal = 0.5f;
    public float averageAgeGoal = 1;
    
    private int _minPop;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void StartGoalSetting()
    {
        _minPop = RuleManager.Instance.MoosePopMin;
        ElgManager instance = ElgManager.instance;
        int mooseToShoot = instance.elg_population - _minPop;
        UpdateExpectedMales((mooseToShoot * instance.GetMaleRatio()).ToString("F0"));
        UpdateExpectedFemales((mooseToShoot * (1 - instance.GetMaleRatio())).ToString("F0"));
        currentHuntingSeason.text = "The current season will last from " + (RuleManager.Month)RuleManager.Instance.huntingSeasonRange.Min() + " to " + (RuleManager.Month)RuleManager.Instance.huntingSeasonRange.Max() + ".";
    }

    public void EndGoalSetting()
    {
        RuleManager.Instance.gameObject.SetActive(true);
        InfoUI.Instance.gameObject.SetActive(true);
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
    private void UpdateExpectedMales(string inString)
    {
        expectedMales.text = inString;
    }
    private void UpdateExpectedFemales(string inString)
    {
        expectedFemales.text = inString;
    }
}
