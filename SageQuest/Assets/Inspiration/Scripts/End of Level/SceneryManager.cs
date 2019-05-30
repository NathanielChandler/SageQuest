using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneryManager : MonoBehaviour 
{

	public void StartGame()
    {
        SceneManager.LoadScene("Dev");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Starting Main Menu");
    }
    public void LevelMenu()
    {
        SceneManager.LoadScene("");
    }
    public void StartScene()
    {
        SceneManager.LoadScene("");
    }
    public void LoadingScene()
    {
        SceneManager.LoadScene("");
    }
}
