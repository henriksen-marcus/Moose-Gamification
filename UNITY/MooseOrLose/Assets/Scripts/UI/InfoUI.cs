using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoUI : UI
{
    public static InfoUI Instance;
    
    public bool gridOn = true;
    [Header("Moose")]
    [SerializeField] private TextMeshProUGUI mooseCount;
    [SerializeField] private TextMeshProUGUI mMaleCount;
    [SerializeField] private TextMeshProUGUI mFemaleCount;
    [SerializeField] private TextMeshProUGUI mChildCount;
    [SerializeField] private TextMeshProUGUI mMaleRatio;
    [SerializeField] private TextMeshProUGUI mMaleRatioGoal;
    [SerializeField] private TextMeshProUGUI mMaleAge;
    [SerializeField] private TextMeshProUGUI mMaleAgeGoal;
    [Header("Hunters")]
    [SerializeField] private TextMeshProUGUI hunterCount;
    [Header("Wolves")]
    [SerializeField] private TextMeshProUGUI wolfCount;

    private void Start()
    {
        ElgManager.instance.OnPopulationChanged += UpdateCount/*Agent.Moose*/;
        // TODO bind JegerManager and UlvManager events to functions to update text
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void UpdateCount(/*Agent agent*/)
    {
        ElgManager instance = ElgManager.instance;
        mooseCount.text = instance.elg_population.ToString();
        mMaleCount.text = instance.elg_males.ToString();
        mFemaleCount.text = instance.elg_females.ToString();
        mChildCount.text = instance.elg_children.ToString();
        mMaleAge.text = instance.GetMalePopulationAge().ToString("F2");
        mMaleAge.color = Math.Abs(instance.GetMalePopulationAge() - float.Parse(mMaleAgeGoal.text)) > 0.1f ? Color.red : Color.white;
        mMaleRatio.text = instance.GetMaleRatio().ToString("F2");
        mMaleRatio.color = Math.Abs(instance.GetMaleRatio() - float.Parse(mMaleRatioGoal.text)) > 0.03f ? Color.red : Color.white;
    }

    public void UpdateRatioGoal(float ratio)
    {
        mMaleRatioGoal.text = ratio.ToString("F2");
        
    }
    public void UpdateAgeGoal(float age)
    {
        mMaleAgeGoal.text = age.ToString("F2");
    }
    
    public void ToggleGrid()
    {
        gridOn = !gridOn;
    }

    public void UpdateRatioGoal(string inString)
    {
        mMaleRatioGoal.text = inString;
    }
    
    // private enum Agent
    // {
    //     Moose = 0,
    //     Hunter = 1,
    //     Wolf = 2
    // }
}
