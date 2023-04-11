using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverableUIElement : MonoBehaviour
{

    [SerializeField] private string text;
 
    public void SetText(string input) { text = input; }


    public void MouseEnter()
    {
        Tooltip.instance.ShowToolTip(text);
    }

    public void MouseExit()
    {
        Tooltip.instance.HideToolTip();
    }
}
