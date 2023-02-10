using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Measures the time taken to execute something.
/// </summary>
public class Timer
{
    private float _time;
    private string _msg;

    public void Start()
    {
        _time = Time.realtimeSinceStartup;
    }

    public void Start(string message)
    {
        _msg = message;
        Start();
    }

    public float GetTime()
    {
        return Time.realtimeSinceStartup - _time;
    }

    public override string ToString()
    {
        return _msg + ": " + GetTime();
    }
}
