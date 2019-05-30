using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public GameObject audioItemSFX;
    public GameObject audioItemSFX_Loop;
    public GameObject audioItemMX;
    public GameObject audioItemVOX;

    private GameObject prefabBus;

    public AudioClip eightiesSong;
    public AudioClip ninetiesSong;
    public AudioClip twoThousandsSong;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        Scene scene = SceneManager.GetActiveScene();

        if (scene.name.Contains("1-"))
        {
            LoopPlay(eightiesSong, "mx");
        }
        if (scene.name.Contains("2-"))
        {
            LoopPlay(ninetiesSong, "mx");
        }
        if (scene.name.Contains("3-"))
        {
            LoopPlay(twoThousandsSong, "mx");
        }
    }
    
    
    public void Play(AudioClip clip, string bus)
    {
        if (bus == "sfx")
        {
            prefabBus = audioItemSFX;
        }
        if (bus == "vox")
        {
            prefabBus = audioItemVOX;
        }

        GameObject go = (GameObject)Instantiate(prefabBus);
        AudioSource src = go.GetComponent<AudioSource>();
        src.clip = clip;
        src.Play();
        Destroy(go, clip.length);
    }
    public void LoopPlay(AudioClip clip, string bus)
    {
        if (bus == "mx")
        {
            prefabBus = audioItemMX;
        }
        if (bus == "sfx_l")
        {
            prefabBus = audioItemSFX_Loop;
        }
        GameObject go = (GameObject)Instantiate(prefabBus);
        AudioSource src = go.GetComponent<AudioSource>();
        src.clip = clip;
        src.Play();
    }
    public void Stop(string bus)
    {
        if (bus == "mx")
        {
            prefabBus = audioItemMX;
        }
        if (bus == "sfx")
        {
            prefabBus = audioItemSFX;
        }
        if (bus == "sfx_l")
        {
            prefabBus = audioItemSFX_Loop;
        }
        prefabBus.GetComponent<AudioSource>().Stop();
    }
}