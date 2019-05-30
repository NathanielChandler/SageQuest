using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private int score = 0;
    public int Score { get { return score; } set { score = value; } }

    public Levels levels;

    public Level CurrentLevel()
    {
        Level level = null;

        if (levels != null)
        {
            
            string currentSceneName = SceneManager.GetActiveScene().name;
            foreach (Level l in levels.levels)
            {
                if (l.sceneName == currentSceneName)
                {
                    level = l;
                }
            }
        }

        return level;
    }

    //Save/Load Data
    public void SaveScore()
    {
        //Rework to save high score for every level
        //PlayerPrefs.SetInt("High Score", score);
        //To read a score
        //score = PlayerPrefs.GetInt("High Score");

        if(levels != null)
        {
            string levelHighScoreKey = CurrentLevel().levelName + "_HighScore";

            PlayerPrefs.SetInt(levelHighScoreKey, score);
            PlayerPrefs.Save();
        }
    }

    public int GetScore()
    {
        int levelScore = 0;

        if (levels != null)
        {
            string levelHighScoreKey = CurrentLevel().levelName + "_HighScore";
          
            if (PlayerPrefs.HasKey(levelHighScoreKey))
            {
                levelScore = PlayerPrefs.GetInt(levelHighScoreKey);
            }
        }

        return levelScore;
    }
}