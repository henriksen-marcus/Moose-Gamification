using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
{
    static public TimeManager instance;

    [SerializeField] int month;
    [SerializeField] int day;
    [SerializeField] int year;
    public float playSpeed = 15;
    public float defaultPlaySpeed = 3;
    [HideInInspector]
    public float startPlaySpeed;




    [SerializeField] public TextMeshProUGUI dayUI;
    [SerializeField] public TextMeshProUGUI monthUI;
    [SerializeField] public TextMeshProUGUI yearUI;

    private bool springbegun = false;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject Canvas = GameObject.Find("UI_Canvas");
        dayUI = Canvas.transform.Find("Clock").transform.Find("Background").transform.Find("Day").GetComponent<TextMeshProUGUI>();
        monthUI = Canvas.transform.Find("Clock").transform.Find("Background").transform.Find("Month").GetComponent<TextMeshProUGUI>();
        yearUI = Canvas.transform.Find("Clock").transform.Find("Background").transform.Find("Year").GetComponent<TextMeshProUGUI>();

        if (instance == null)
        {
            instance = this;
        }
        day = 0;
        month = 5;
        year = 0;
        StartCoroutine(NextDay());

        startPlaySpeed = defaultPlaySpeed;
    }

    private void Update()
    {
        if (yearUI != null && monthUI != null && dayUI != null)
        {
            yearUI.SetText(year.ToString());
            switch (month)
            {
                case 0:
                    monthUI.SetText("Jan");
                    break;
                case 1:
                    monthUI.SetText("Feb");
                    break;
                case 2:
                    monthUI.SetText("Mar");
                    break;
                case 3:
                    monthUI.SetText("Apr");
                    break;
                case 4:
                    monthUI.SetText("May");
                    break;
                case 5:
                    monthUI.SetText("Jun");
                    break;
                case 6:
                    monthUI.SetText("Jul");
                    break;
                case 7:
                    monthUI.SetText("Aug");
                    break;
                case 8:
                    monthUI.SetText("Sep");
                    break;
                case 9:
                    monthUI.SetText("Oct");
                    break;
                case 10:
                    monthUI.SetText("Nov");
                    break;
                case 11:
                    monthUI.SetText("Des");
                    break;

                default:
                    break;
            }
            dayUI.SetText((day+1).ToString());
        }
    }
    public int GetYear() {  return year; }
    public int GetMonth() {  return month; }
    public int GetDay() { return day; }
    public bool MatingSeason() { return month == 7 || month == 8; }

    public bool HuntingSeason() { return month == 9 || month == 10 || month == 11; }

    public bool IsSummer() { return month == 5 || month == 6 || month == 7; }
    public bool IsWinter() { return month == 11 || month == 0 || month == 1; }
    public bool IsSpring() { return month == 2 || month == 3 || month == 4; }
    public bool IsAutumn() { return month == 8 || month == 9 || month == 10; }

    // Update is called once per frame
    IEnumerator NextDay()
    {
        yield return new WaitForSeconds(playSpeed);
        NewDay();

        day++;
        
        if (day > 29)
        {
            if (month > 1 && !springbegun)
            {
                springbegun = true;
                SpringBegin();
            }
            ElgManager.instance.SetMalePopulationAge();
            month++;
            day = 0;
        }
        if (month > 11)
        {
            springbegun = false;
            NewYear();
            year++;
            month = 0;
        }

        
        StartCoroutine(NextDay());
    }

    public event Action OnNewDay;
    public void NewDay()
    {
        if (OnNewDay != null)
        {
            OnNewDay();
        }
    }


    public event Action OnNewYear;
    public void NewYear()
    {
        if (OnNewYear != null)
        {
            OnNewYear();
        }
    }

    public event Action OnSpringBegin;
    public void SpringBegin()
    {
        if (OnSpringBegin != null)
        {
            OnSpringBegin();
        }
    }
}
