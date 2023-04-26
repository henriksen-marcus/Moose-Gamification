using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string SceneName;

    public void StartGame()
    {
        SceneManager.LoadScene(SceneName);
    }

    public void ExitGame() => Application.Quit();
}
