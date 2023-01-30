using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    static public TimeManager instance;

    [SerializeField] int month;
    [SerializeField] int day;
    [SerializeField] int year;
    public int playSpeed = 15;
    [HideInInspector]
    public int startPlaySpeed;


    [SerializeField] public TextMeshProUGUI dayUI;
    [SerializeField] public TextMeshProUGUI monthUI;
    [SerializeField] public TextMeshProUGUI yearUI;

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
        StartCoroutine(NextDay());

        startPlaySpeed = playSpeed;
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
            dayUI.SetText(day.ToString());
        }
    }
    public int GetYear() {  return year; }
    public int GetMonth() {  return month; }
    public int GetDay() { return day; }
    public bool MatingSeason() { return month == 7 || month == 8; }

    // Update is called once per frame
    IEnumerator NextDay()
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

        yield return new WaitForSeconds(playSpeed);
        StartCoroutine(NextDay());
    }
}
