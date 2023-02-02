using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContainer : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    // private void Start()
    // {
    //     RuleManager.OnExpand += Rebuild;
    // }

    // private void Rebuild()
    // {
    //     LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    // }

    private void Update()
    {
        //terrible and expensive fix TODO: replace this lol
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
    }
}
