using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPlayController : MonoBehaviour
{

    public static UIPlayController instance;

    TextMeshProUGUI playSpeed;
    TextMeshProUGUI playPause;
    // Start is called before the first frame update
    void Awake()
    {
        playSpeed = transform.Find("PlaySpeed").GetComponent<TextMeshProUGUI>();
        playPause = transform.Find("PlayPause").transform.Find("Text").GetComponent<TextMeshProUGUI>();
        playPause.text = "Pause";

        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        switch (TimeManager.instance.playSpeed)
        {
            case 0.5f:
                playSpeed.text = "x4";
                break;
            case 1f:
                playSpeed.text = "x3";
                break;
            case 2f:
                playSpeed.text = "x2";
                break;
            case 4f:
                playSpeed.text = "x1";
                break;
            default:
                break;
        }
    }

    public void TogglePlay()
    {
        TimeManager.instance.TogglePlay();       
    }

    public void UpdateTexts()
    {
        if (TimeManager.instance.gamePaused)
        {
            playPause.text = "Play";
        }
        else
        {
            playPause.text = "Pause";
        }
    }
    public void SpeedUp()
    {
        TimeManager.instance.SpeedUp();
        switch(TimeManager.instance.playSpeed)
        {
            case 0.5f:
                playSpeed.text = "x4";
                break;
            case 1f:
                playSpeed.text = "x3";
                break;
            case 2f:
                playSpeed.text = "x2";
                break;
            case 4f:
                playSpeed.text = "x1";
                break;
            default:
                break;
        }
    }
    public void SlowDown()
    {
        TimeManager.instance.SlowDown();
        switch (TimeManager.instance.playSpeed)
        {
            case 0.5f:
                playSpeed.text = "x4";
                break;
            case 1f:
                playSpeed.text = "x3";
                break;
            case 2f:
                playSpeed.text = "x2";
                break;
            case 4f:
                playSpeed.text = "x1";
                break;
            default:
                break;
        }
    }
}
