using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Calendar : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI dayUI;
    [SerializeField] public TextMeshProUGUI monthUI;
    [SerializeField] public TextMeshProUGUI yearUI;

    public string[] monthNames = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Des" };
    // Start is called before the first frame update
    void Start()
    {
        dayUI = transform.Find("Day").GetComponent<TextMeshProUGUI>();
        monthUI = transform.Find("Month").GetComponent<TextMeshProUGUI>();
        yearUI = transform.Find("Year").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        yearUI.SetText(TimeManager.instance.GetYear().ToString());
        monthUI.SetText(monthNames[TimeManager.instance.GetMonth()]);
        dayUI.SetText((TimeManager.instance.GetDay() + 1).ToString());
    }
}
