using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject SaveGame;
    private GameObject saveResultsPanel;

    private void Awake()
    {
        if (instance == null)  instance = this;

        Menu.gameObject.SetActive(false);
        
    }

    private void Start()
    {
        TimeManager.Instance.Pause += ToggleMenu;
    }
    
    
    private void ToggleMenu(bool paused)
    {
        Menu.SetActive(paused);
    }

    // Like I said, we are using the new input system, so this should not be used.
    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }*/

    public void Close()
    {
        TimeManager.Instance.OnPause(false);
        Menu.SetActive(false);
    }

    public void OpenSaveGameUI()
    {
        saveResultsPanel = Instantiate(SaveGame, GameObject.Find("Screen Canvas").transform);
    }

    public void Exit()
    {
        Application.Quit();
    }

    /*public void Toggle()
    {
        TimeManager.instance.SetGamePaused(!TimeManager.instance.gamePaused);
        ToggleMenu();
        if (saveResultsPanel)
        {
            Destroy(saveResultsPanel);
            saveResultsPanel = null;
        }
    }*/
}
