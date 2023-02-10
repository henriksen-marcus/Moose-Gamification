using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public enum Season { Spring, Summer, Autumn, Winter }

public class TimeManager : MonoBehaviour
{
    static public TimeManager instance;

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

    private Season currentSeason;
    private bool gamePaused = false;
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
        month = startMonth;
        year = 0;
        StartCoroutine(NextDay());

        defaultPlaySpeed = 3;
        startPlaySpeed = defaultPlaySpeed;
        gamePaused = false;
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

    public bool IsSummer() { return currentSeason == Season.Summer; }
    public bool IsWinter() { return currentSeason == Season.Winter; }
    public bool IsSpring() { return currentSeason == Season.Spring; }
    public bool IsAutumn() { return currentSeason == Season.Autumn; }

    public void SetGamePaused(bool input) {  gamePaused = input; }

    public Season GetSeason()
    {
        return currentSeason;
    }


    // Update is called once per frame
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
                currentSeason = Season.Winter;
                break;
            case 1:
                currentSeason = Season.Winter;
                break;
            case 2:
                currentSeason = Season.Spring;
                SpringBegin();
                break;
            case 3:
                currentSeason = Season.Spring;
                break;
            case 4:
                currentSeason = Season.Spring;
                break;
            case 5:
                currentSeason = Season.Summer;
                break;
            case 6:
                currentSeason = Season.Summer;
                break;
            case 7:
                currentSeason = Season.Summer;
                break;
            case 8:
                currentSeason = Season.Autumn;
                break;
            case 9:
                currentSeason = Season.Autumn;
                break;
            case 10:
                currentSeason = Season.Autumn;
                break;
            case 11:
                currentSeason = Season.Winter;
                break;

            default:
                break;
        }


        if (OnNewMonth != null)
        {
            OnNewMonth();
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
