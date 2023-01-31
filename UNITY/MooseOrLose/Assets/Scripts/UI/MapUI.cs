using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    public static bool GridOn = true;

    public void ToggleGrid()
    {
        GridOn = !GridOn;
    }

}
