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
    
    
    
    public void ToggleGrid()
    {
        GridOn = !GridOn;
    }

}
