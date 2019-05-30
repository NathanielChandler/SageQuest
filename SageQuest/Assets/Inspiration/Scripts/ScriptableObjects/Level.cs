using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelObject", menuName = "LevelObjects", order = 2)]
public class Level : ScriptableObject
{
    public string levelName;
    public string levelCreator;
    public Sprite levelPreview;
    public float highestScore;
    public string sceneName;
    public bool unlocked;
}
