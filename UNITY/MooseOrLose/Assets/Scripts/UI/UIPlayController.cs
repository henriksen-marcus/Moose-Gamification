using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void TogglePlay()
    {
        TimeManager.instance.TogglePlay();
    }
    public void SpeedUp()
    {
        TimeManager.instance.SpeedUp();
    }
    public void SlowDown()
    {
        TimeManager.instance.SlowDown();
    }
}
