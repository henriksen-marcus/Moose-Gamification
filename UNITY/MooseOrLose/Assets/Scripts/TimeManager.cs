using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    [SerializeField] int month;
    [SerializeField] int day;
    [SerializeField] int year;
    [SerializeField] int startMonth;
    public float playSpeed = 15;
    public float defaultPlaySpeed = 3;
    [HideInInspector]
    public float startPlaySpeed;

    [SerializeField] public TextMeshProUGUI dayUI;
    [SerializeField] public TextMeshProUGUI monthUI;
    [SerializeField] public TextMeshProUGUI yearUI;
    
    public Season currentSeason { get; private set; }
    
    private bool gamePaused = false;

    public string[] monthNames = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Des" };

    void Awake()
    {
        GameObject Canvas = GameObject.Find("UI_Canvas");
        
        dayUI = Canvas.transform.Find("Clock").transform.Find("Background").transform.Find("Day").GetComponent<TextMeshProUGUI>();
        monthUI = Canvas.transform.Find("Clock").transform.Find("Background").transform.Find("Month").GetComponent<TextMeshProUGUI>();
        yearUI = Canvas.transform.Find("Clock").transform.Find("Background").transform.Find("Year").GetComponent<TextMeshProUGUI>();

        if (!instance) instance = this;
        
        day = 0;
        month = startMonth;
        year = 0;
        
        StartCoroutine(NextDay());

        defaultPlaySpeed = 3;
        startPlaySpeed = defaultPlaySpeed;
        gamePaused = false;
    }

    private void Update()
    {
        yearUI.SetText(year.ToString());
        monthUI.SetText(monthNames[month - 1]);
        dayUI.SetText((day+1).ToString());

    }
    public int GetYear() { return year; }
    public int GetMonth() { return month; }
    public int GetDay() { return day; }
    public bool MatingSeason() { return month == 7 || month == 8; }

    // TODO: Denne koden er ubrukt, fjern dersom den ikke skal brukes
    public bool IsSummer() { return currentSeason == Season.Summer; }
    public bool IsWinter() { return currentSeason == Season.Winter; }
    public bool IsSpring() { return currentSeason == Season.Spring; }
    public bool IsAutumn() { return currentSeason == Season.Fall; }

    public void SetGamePaused(bool input) {  gamePaused = input; }

    IEnumerator NextDay()
    {
        yield return new WaitForSeconds(playSpeed);
        if (!gamePaused)
        {
            day++;
            NewDay();

            if (day > 29)
            {
                ElgManager.instance.SetMalePopulationAge();
                month++;
                NewMonth();
                day = 0;
            }
            if (month > 11)
            {
                year++;
                NewYear();
                month = 0;
            }
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

    public event Action OnNewMonth;
    public void NewMonth()
    {
        switch (month)
        {
            case 0:
            case 1:
            case 11:
                currentSeason = Season.Winter;
                break;
            case 2:
                currentSeason = Season.Spring;
                SpringBegin();
                break;
            case 3:
            case 4:
                currentSeason = Season.Spring;
                break;
            case 5:
            case 6:
            case 7:
                currentSeason = Season.Summer;
                break;
            case 8:
            case 9:
            case 10:
                currentSeason = Season.Fall;
                break;
        }

        OnNewMonth?.Invoke();
    }
    
    public event Action OnNewYear;
    public void NewYear()
    {
        OnNewYear?.Invoke();
    }

    public event Action OnSpringBegin;
    public void SpringBegin()
    {
        OnSpringBegin?.Invoke();
    }
}
