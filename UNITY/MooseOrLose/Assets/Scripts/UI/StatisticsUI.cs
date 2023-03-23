using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsUI : MonoBehaviour
{
    public static StatisticsUI Instance;

    [SerializeField] private Graph graph;
    
    public bool showStatistics = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void SetShow(bool show)
    {
        showStatistics = show;
    }

    public void UpdateGraph()
    {
        graph.UpdateGraphMenu();
    }
}
