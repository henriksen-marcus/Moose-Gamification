using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverableUIElement : MonoBehaviour
{

    private string text;
 
    public void SetText(string input) { text = input; }

    // Update is called once per frame
    public void MouseEnter()
    {
        Tooltip.instance.ShowToolTip(text);
    }

    // ...and the mesh finally turns white when the mouse moves away.
    public void MouseExit()
    {
        Tooltip.instance.HideToolTip();
    }
}
