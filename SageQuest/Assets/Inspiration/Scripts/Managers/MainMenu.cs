using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;



public class MainMenu : MonoBehaviour
{
    public enum Panels
    {
        Home,
        LevelSelect,    
        Options,
        Credits,
        LoadingScreen,
        LevelScreen
    }

    public enum Era
    {
        The80s,
        The90s,
        The00s
    }

    [Header("Panels")]
    public Panels currentPanel = Panels.Home;
    public GameObject[] panels; //Index the same as enums
    public List<Panels> panelHistory = new List<Panels>();

    [Header("Volume UI")]
    public AudioMixer mixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider effectSlider;

    [Header("Tips")]
    public Tips tips;
    public Text tipsText;

    [Header("Level Selection")]
    public Era era = Era.The80s;
    int currentLevelIndex;
    public Levels levels;
    private Level levelToLoad;
    public Text levelNameText;
    public Text highscoreText;
    public Text levelCreatorText;
    public Button playButton;

    public Button Button80s;
    public Button Button90s;
    public Button Button00s;

    public RectTransform arrowIcon;

    public Button[] levelButtons;

    float masterVolume;
    float musicVolume;
    float sfxVolume;

    bool asyncLoading;

    public AudioClip buttonPush;

    private int austinEra = 0;
    private int austinLevel = 1;
    private string levelName;

    void Start()
    {
        //PlayerPrefs.DeleteAll();

        LoadUnlockedLevels();
        InitLevels();

        masterVolume = GetVolume("MasterVolume");
        musicVolume = GetVolume("MusicVolume");
        sfxVolume = GetVolume("SfxVolume");

        masterSlider.value = masterVolume;
        musicSlider.value = musicVolume;
        effectSlider.value = sfxVolume;

        masterSlider.onValueChanged.AddListener(delegate { OnMasterVolumeChanged(); });
        musicSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChanged(); });
        effectSlider.onValueChanged.AddListener(delegate { OnSfxVolumeChanged(); });

        //??????????????????

        if (levels.levels[5].unlocked)
        {
            //90s Unlocked
            if (Button90s != null)
                Button90s.interactable = true;
        }

        if (levels.levels[10].unlocked)
        {
            //00's Unlocked
            if (Button00s != null)
                Button00s.interactable = true;
        }

        //ChangeEra(0);
    }

    //Ultra lazy code.
    public void ChangeEra(int newEra)
    {
        era = (Era)newEra;
        
        switch(era)
        {
            case Era.The80s:

                arrowIcon.anchoredPosition3D = new Vector3(-37.5f, 45, 0);

                for (int i = 0; i < 5; i++)
                {
                    if(levels.levels[i].unlocked)
                    {
                        levelButtons[i].interactable = true;
                    }
                }
                SoundManager.instance.Play(buttonPush, "sfx");
                austinEra = 0;
                break;
            case Era.The90s:

                arrowIcon.anchoredPosition3D = new Vector3(-37.5f, -30, 0);

                for (int i = 5; i < 10; i++)
                {
                    if (levels.levels[i-5].unlocked)
                    {
                        levelButtons[i-5].interactable = true;
                    }
                }
                SoundManager.instance.Play(buttonPush, "sfx");
                austinEra = 1;
                break;
            case Era.The00s:

                arrowIcon.anchoredPosition3D = new Vector3(-37.5f, -105, 0);

                for (int i = 10; i < 15; i++)
                {
                    if (levels.levels[i-10].unlocked)
                    {
                        levelButtons[i-10].interactable = true;
                    }
                }
                SoundManager.instance.Play(buttonPush, "sfx");
                austinEra = 2;
                break;
        }
    }

    //Austin Levels
    public void level1 () {
        if (austinEra == 0) {
            LoadScene ("Level_1-1");
        }
        if (austinEra == 1) {
            LoadScene ("2-1Jesus");
        }
        if (austinEra == 2) {
            LoadScene ("3-1");
        }
    }
    public void level2 () {
        if (austinEra == 0) {
            LoadScene ("1-2");
        }
        if (austinEra == 1) {
            LoadScene ("Darren'sLevel2-2");
        }
        if (austinEra == 2) {
            LoadScene ("Promero3-Whatever");
        }
    }
    public void level3 () {
        if (austinEra == 0) {
            LoadScene ("Level_1-1");
        }
        if (austinEra == 1) {
            LoadScene ("Level_1-1");
        }
        if (austinEra == 2) {
            LoadScene ("Level_1-1");
        }
    }
    public void level4 () {
        if (austinEra == 0) {
            LoadScene ("Level_1-1");
        }
        if (austinEra == 1) {
            LoadScene ("Level_1-1");
        }
        if (austinEra == 2) {
            LoadScene ("Level_1-1");
        }
    }
    public void level5 () {
        if (austinEra == 0) {
            LoadScene ("Level_1-1");
        }
        if (austinEra == 1) {
            LoadScene ("Level_1-1");
        }
        if (austinEra == 2) {
            LoadScene ("Level_1-1");
        }
    }

    public void SelectLevel(int level)
    {
        currentLevelIndex = level + ((int)era * 5);

        levelToLoad = levels.levels[currentLevelIndex];

        //levelNameText.text = "LEVEL " + levelToLoad.levelName;

        //string highscoreKey = levelToLoad.levelName + "_HighScore";

       /*if(PlayerPrefs.HasKey(highscoreKey))
        {
            highscoreText.text = "Highscore: " + PlayerPrefs.GetString(highscoreKey);
        }
        else
        {
            highscoreText.text = "Highscore: 0";
        }  */

        LoadLevel ();
    }

    void OnMasterVolumeChanged()
    {
        SetVolume(ref masterVolume, ref masterSlider, "MasterVolume");
    }

    void OnMusicVolumeChanged()
    {
        SetVolume(ref musicVolume, ref musicSlider, "MusicVolume");
    }

    void OnSfxVolumeChanged()
    {
        SetVolume(ref sfxVolume, ref effectSlider, "SfxVolume");
    }

    void SetVolume(ref float volumeType, ref Slider slider, string mixerVariable)
    {
        volumeType = slider.value;
        mixer.SetFloat(mixerVariable, volumeType);
        PlayerPrefs.SetFloat(mixerVariable, volumeType);
        PlayerPrefs.Save();
    }

    float GetVolume(string vol)
    {
        if (PlayerPrefs.HasKey(vol))
        {
            return PlayerPrefs.GetFloat(vol);
        }
        else
        {
            return 0f;
        }
    }

    public void LastPanel()
    {
        int lastPanel = panelHistory.Count - 1;
        ChangePanel(panelHistory[lastPanel]);
        panelHistory.RemoveAt(lastPanel);
    }

    public void ChangePanel(int panelIndex)
    {
        panelHistory.Add(currentPanel);
        ChangePanel((Panels)panelIndex);
    }

    public void ChangePanel(Panels newPanel)
    {
        panels[(int)currentPanel].SetActive(false);
        currentPanel = newPanel;
        panels[(int)currentPanel].SetActive(true);
        SoundManager.instance.Play(buttonPush, "sfx");
    }

    public void HideAllPanels()
    {
        foreach (var item in panels)
        {
            item.SetActive(false);
        }
    }

    //Allow Button Events to access these functions.
    public void LoadLevel() //Load from levelToLoad data
    {
        AsyncLoadScene(levelToLoad.sceneName);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void AsyncLoadScene(string sceneName)
    {
        StartCoroutine(AsyncLoadSceneCoroutine(sceneName));
    }

    IEnumerator AsyncLoadSceneCoroutine(string sceneName)
    {
        ChangePanel(Panels.LoadingScreen);

        if(tipsText)
        {
            int random = Random.Range(0, tips.tips.Length);
            tipsText.text = tips.tips[random];
        }

        yield return new WaitForSeconds(1f);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while(!asyncOperation.isDone)
        {          
            yield return null;
        }

        HideAllPanels();
    }

    void LoadUnlockedLevels()
    {
        for (int i = 0; i < levels.levels.Length; i++)
        {
            string levelUnlockKey = levels.levels[i].levelName + "_Unlocked";

            if (PlayerPrefs.HasKey(levelUnlockKey))
            {
                int unlocked = PlayerPrefs.GetInt(levelUnlockKey);

                if (unlocked == 0)
                {
                    levels.levels[i].unlocked = false;
                }
                else if (unlocked == 1)
                {
                    levels.levels[i].unlocked = true;
                }

                //Only for 1-1
                if (i == 0)
                {
                    levels.levels[i].unlocked = true;
                }
            }
            else
            {
                if (i != 0)
                {
                    PlayerPrefs.SetInt(levelUnlockKey, 0);
                    PlayerPrefs.Save();
                }
            }
        }
    }

    //Level Functions
    //Can refactor in events later on if need be
    void InitLevels()
    {
        levelToLoad = levels.levels[0];
        UpdateLevel();
    }

    void UpdateLevel()
    {
        levelNameText.text = levelToLoad.levelName;
        levelCreatorText.text = string.Format("Created by: {0}", levelToLoad.levelCreator);

        if(!levelToLoad.unlocked)
        {
            playButton.interactable = false;
        }
        else
        {
            playButton.interactable = true;
        }
    }

    void ChangeLevel(int index)
    {
        if(index < 0)
        {
            index = levels.levels.Length - 1;
        }

        if(index > levels.levels.Length - 1)
        {
            index = 0;
        }

        currentLevelIndex = index;
        levelToLoad = levels.levels[currentLevelIndex];
        UpdateLevel();
    }

    public void NextLevel()
    {
        ChangeLevel(currentLevelIndex + 1);
    }

    public void PreviousLevel()
    {
        ChangeLevel(currentLevelIndex - 1);
    }
}
