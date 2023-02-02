using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class RuleManager : UI, IPointerEnterHandler
{
    // Singleton
    public static RuleManager Instance;
    
    //moving rules from other managers here?
    private List<int> _huntingSeasonRange;
    private bool _huntingSeason;

    public static event Action OnExpand;
    
    private void Start()
    {
        Shrink();
        Debug.Log("Rule shrink");
        InventoryUI.OnExpand += Shrink;
    }

    public bool HuntingSeason()
    {
        return _huntingSeasonRange.Contains(TimeManager.instance.GetMonth());
    }

    public void SetHuntingSeasonRange(List<int> range)
    {
        // _huntingSeasonRange.Add(range.Item1);
        // int i = 1 + (range.Item2 - range.Item1);
        // for (int a = 0; a < i; a++)
        // {
        //     _huntingSeasonRange.Add();
        // }
        _huntingSeasonRange.AddRange(range);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Expand();
        Debug.Log("Rule expand");
        OnExpand?.Invoke();
    }
}
