using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScene : MonoBehaviour
{
    public GameObject gameM;
    public GameObject victoryScreen;
    public GameObject credits;
    public GameObject companyLogo;
    public GameObject gameLogo;
    public GameObject story;

    public bool openingSceneDone = false;
    public bool gameDone = false;
    public bool creditsDone = false;
    public bool inGame = false;
    public bool inCredits = false;
    public float SceneTimer;
    public float gameLogoTimer;
    public int herb;

    // Use this for initialization
    void Start()
    {
        SceneTimer = 15;
        gameLogoTimer = 3;
    }

    // Update is called once per frame
    void Update()
    {
        CheckScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("Prototyp Level");
    }

    private void LoadEndingScene()
    {
        SceneManager.LoadScene("Ending");
    }

    private void CheckScene()
    {
        herb = gameM.GetComponent<GameManager>().herbsGathered;
        if(herb >= 4)
        {
            gameDone = true;
        }
        if (openingSceneDone == false)
        {
            OpeningScene();
        }
        if (openingSceneDone == true && inGame == false)
        {
            LoadGameScene();
        }
        if (gameDone == true)
        {
            LoadEndingScene();
        }
    }
    private void OpeningScene()
    {
        gameLogoTimer -= 1 * Time.deltaTime;
        SceneTimer -= 1 * Time.deltaTime;
        if (gameLogoTimer >= 3)
        {
            companyLogo.SetActive(false);
            gameLogo.SetActive(true);

        }
        if (SceneTimer <= 12 && gameLogoTimer <= 0)
        {
            companyLogo.SetActive(false);
            gameLogo.SetActive(true);
        }
        if (SceneTimer <= 10)
        {
            gameLogo.SetActive(false);
            story.SetActive(true);
        }
        if (SceneTimer <= 0)
        {
            openingSceneDone = true;
        }
    }

    private void EndingScene()
    {
        SceneTimer -= 1 * Time.deltaTime;
        if (SceneTimer <= 0)
        {
            victoryScreen.SetActive(false);
            credits.SetActive(true);
        }
    }
}