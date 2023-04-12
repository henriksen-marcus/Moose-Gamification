using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Globalization;

public class InfoUI : UI
{
    public static InfoUI Instance;
    
    public bool gridOn = false;
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

    [SerializeField] private TextMeshProUGUI mSquareKm;
    [SerializeField] private TextMeshProUGUI mSquareKmGoal;

    private void Start()
    {
        ElgManager.instance.OnPopulationChanged += UpdateCount/*Agent.Moose*/;
        RuleManager.Instance.OnHuntingSeasonStart += ResetSquareKm;
        // TODO bind JegerManager and UlvManager events to functions to update text
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void ResetSquareKm()
    {
        mSquareKm.text = 0.ToString();
    }

    private float GetSquareKm()
    {
        if (float.TryParse(mSquareKm.text, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
        {
            return result;
        }
        else
        {
            return 0;
        }
    }
    
    public void AddToSquareKm()
    {
        if (float.TryParse(mSquareKm.text, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
        {
            mSquareKm.text = (result + 0.067f).ToString(CultureInfo.CurrentCulture);
            if (float.TryParse(mSquareKmGoal.text, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float resultGoal))
            {
                mSquareKm.color = Math.Abs(GetSquareKm() - resultGoal) > 0.1f ? Color.red : Color.white;
            }
        }
        else
        {
            mSquareKm.text = 0.ToString();
        }
    }
    
    private void UpdateCount(/*Agent agent*/)
    {
        ElgManager instance = ElgManager.instance;
        mooseCount.text = instance.elg_population.ToString();
        mMaleCount.text = instance.elg_males.ToString();
        mFemaleCount.text = instance.elg_females.ToString();
        mChildCount.text = instance.elg_children.ToString();
        

        if (instance.GetMalePopulationAge().ToString().Length > 4)
        {
            mMaleAge.text = instance.GetMalePopulationAge().ToString().Remove(4, instance.GetMalePopulationAge().ToString().Length - 4);
        }
        else
        {
            mMaleAge.text = instance.GetMalePopulationAge().ToString();
        }
        
        string text = mMaleAgeGoal.text.ToString(CultureInfo.InvariantCulture);
        if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out float result))
        {
            mMaleAge.color = Math.Abs(instance.GetMalePopulationAge() - result) > 0.1f ? Color.red : Color.white;
        }

        string maleratio = instance.GetMaleRatio().ToString();
        if (maleratio.Length > 4)
        {
            mMaleRatio.text = maleratio.Remove(4, instance.GetMaleRatio().ToString().Length - 4);
        }
        else
        {
            mMaleRatio.text = instance.GetMaleRatio().ToString();
        }
        
        text = mMaleRatio.text.ToString(CultureInfo.InvariantCulture);
        if (float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out result))
        {
            mMaleRatio.color = Math.Abs(instance.GetMaleRatio() - result) > 0.03f ? Color.red : Color.white;
        }
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
    public void UpdateSquareKmGoal(float km)
    {
        mSquareKmGoal.text = km.ToString("F2");
    }
    // private enum Agent
    // {
    //     Moose = 0,
    //     Hunter = 1,
    //     Wolf = 2
    // }
}
