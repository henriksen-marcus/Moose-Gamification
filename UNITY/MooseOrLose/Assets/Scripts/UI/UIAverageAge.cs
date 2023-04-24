using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAverageAge : MonoBehaviour
{
    TextMeshProUGUI averageAge;
    // Start is called before the first frame update
    void Start()
    {
        averageAge = GetComponent<TextMeshProUGUI>();
        TimeManager.Instance.OnNewDay += UpdateUI;

    }

    // Update is called once per frame
    void UpdateUI()
    {
        averageAge.text = ElgManager.instance.GetMalePopulationAge().ToString();
    }
}
