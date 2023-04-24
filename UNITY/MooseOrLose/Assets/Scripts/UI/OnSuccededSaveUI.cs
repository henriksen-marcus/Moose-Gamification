using System;
using UnityEngine;

public class OnSuccededSaveUI : MonoBehaviour
{
    private void Start()
    {
        // Destroy this if we unpause
        TimeManager.Instance.Pause += OnGamePaused;
    }

    public void Finish() => Destroy(gameObject);
    
    private void OnGamePaused(bool paused)
    {
        if (!paused && this != null) Destroy(gameObject);
    }
}
