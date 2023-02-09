using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Measures the time taken to execute something.
/// </summary>
public class Timer
{
    private float m_Time;
    private string msg;

    public void Start()
    {
        m_Time = Time.realtimeSinceStartup;
    }

    public void Start(string message)
    {
        msg = message;
        Start();
    }

    public float GetTime()
    {
        return Time.realtimeSinceStartup - m_Time;
    }

    public override string ToString()
    {
        return msg + ": " + GetTime();
    }
    
}
