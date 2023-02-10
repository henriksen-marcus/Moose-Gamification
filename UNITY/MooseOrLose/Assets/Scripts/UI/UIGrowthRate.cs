using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGrowthRate : MonoBehaviour
{
    TextMeshProUGUI GrowthRate;
    // Start is called before the first frame update
    void Start()
    {
        TimeManager.instance.OnNewDay += UpdateText;
        GrowthRate = GetComponent<TextMeshProUGUI>();    
    }

    // Update is called once per frame
    void UpdateText()
    {
        GrowthRate.text = (ElgManager.instance.GetPopulationGrowthRate() / 100f).ToString();
    }
}
