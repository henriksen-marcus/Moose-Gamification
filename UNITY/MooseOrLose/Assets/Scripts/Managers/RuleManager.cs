using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RuleManager : MonoBehaviour
{
    // Singleton
    public static RuleManager Instance;
    
    //moving rules from other managers here?
    private List<int> _huntingSeasonRange;
    private bool _huntingSeason;
    

    public bool HuntingSeason()
    {
        if (_huntingSeasonRange.Contains(TimeManager.instance.GetMonth()))
            return true;
        else
        {
            return false;
        }
    }

    public bool SetHuntingSeasonRange(/*Tuple<int, int> range*/)
    {
        // _huntingSeasonRange.Add(range.Item1);
        // int i = 1 + (range.Item2 - range.Item1);
        // for (int a = 0; a < i; a++)
        // {
        //     _huntingSeasonRange.Add();
        // }
        
    }
}
