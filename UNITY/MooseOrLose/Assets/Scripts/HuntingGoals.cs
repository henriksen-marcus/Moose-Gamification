using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HuntingGoals : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentHuntingSeason;
    [SerializeField] private TextMeshProUGUI expectedMales;
    [SerializeField] private TextMeshProUGUI expectedFemales;

    public static HuntingGoals Instance;
    
    public float ratioGoal = 0.5f;
    public int averageAgeGoal = 1;
    
    private int _minPop;

    private void Awake()
    {
        _minPop = RuleManager.Instance.moosePopMin;
    }

    private void Start()
    {
        ElgManager instance = ElgManager.instance;
        int mooseToShoot = instance.elg_population - _minPop;
        UpdateExpectedMales((mooseToShoot * instance.GetMaleRatio()).ToString());
        UpdateExpectedFemales((mooseToShoot * (1 - instance.GetMaleRatio())).ToString());
    }

    public void UpdateRatioGoal(string inString)
    {
        float.TryParse(inString, out ratioGoal);
    }
    public void UpdateAgeGoal(string inString)
    {
        int.TryParse(inString, out averageAgeGoal);
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
