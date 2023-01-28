using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    static public TimeManager instance;

    [SerializeField] int month;
    [SerializeField] int day;
    [SerializeField] int year;
    public int playSpeed = 1;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        day = 0;
        month = 0;
        year = 0;
        InvokeRepeating("NextDay", 0, playSpeed);


    }


    public int GetYear() {  return year; }
    public int GetMonth() {  return month; }
    public int GetDay() { return day; }
    public bool MatingSeason() { return month == 8 || month == 9; }

    // Update is called once per frame
    void NextDay()
    {
        day++;
        if (day > 30)
        {
            month++;
            day = 0;
        }
        if (month > 11)
        {
            year++;
            month = 0;
        }

    }
}
