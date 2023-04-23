using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    [SerializeField] private GameObject Menu;
    [SerializeField] private GameObject SaveGame;
    private GameObject saveGame;
    private void Awake()
    {
        if (instance == null)  instance = this;

        Menu.gameObject.SetActive(false);
    }
    public void ToggleMenu()
    {
        if (TimeManager.instance.gamePaused)
        {
            Menu.SetActive(true);
        }
        else
        {
            Menu.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    public void Close()
    {
        Menu.SetActive(false);
    }

    public void OpenSaveGameUI()
    {
        saveGame = Instantiate(SaveGame, GameObject.Find("Screen Canvas").transform);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Toggle()
    {
        TimeManager.instance.SetGamePaused(!TimeManager.instance.gamePaused);
        ToggleMenu();
        if (saveGame)
        {
            Destroy(saveGame);
            saveGame = null;
        }
    }
}
