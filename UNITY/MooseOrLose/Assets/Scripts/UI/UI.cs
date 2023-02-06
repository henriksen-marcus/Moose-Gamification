using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private RectTransform UIRect;
    [SerializeField] private RectTransform titleRect;
    
    protected virtual void Shrink()
    {
        Vector3 UIscale = UIRect.localScale;
        Vector3 titleScale = titleRect.localScale;
        Vector2 titlePivot = titleRect.pivot;
        UIscale.Set(1f, 0.2f, 1f);
        titleScale.Set(1f, 5f, 1f);
        titlePivot.Set(0.5f, 1f);
        UIRect.localScale = UIscale;
        titleRect.localScale = titleScale;
        titleRect.pivot = titlePivot;
    }

    protected virtual void Expand()
    {
        Vector3 UIscale = UIRect.localScale;
        Vector3 titleScale = titleRect.localScale;
        Vector2 titlePivot = titleRect.pivot;
        UIscale.Set(1f, 1f, 1f);
        titleScale.Set(1f, 1f, 1f);
        titlePivot.Set(0.5f, 0.5f);
        UIRect.localScale = UIscale;
        titleRect.localScale = titleScale;
        titleRect.pivot = titlePivot;
    }
}
