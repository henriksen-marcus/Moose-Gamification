using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoUI : MonoBehaviour
{
    public static bool GridOn = true;
    [SerializeField] private TextMeshProUGUI mooseCount;
    [SerializeField] private TextMeshProUGUI hunterCount;
    [SerializeField] private TextMeshProUGUI wolfCount;

    private void Start()
    {
        ElgManager.instance.OnPopulationChanged += UpdateCount/*Agent.Moose*/;
    }

    private void UpdateCount(/*Agent agent*/)
    {
        mooseCount.text = ElgManager.instance.elg_population.ToString();
    }
    
    public void ToggleGrid()
    {
        GridOn = !GridOn;
    }

    // private enum Agent
    // {
    //     Moose = 0,
    //     Hunter = 1,
    //     Wolf = 2
    // }
}
