using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loadout", menuName = "Loadout", order = 1)]
public class Loadout : ScriptableObject
{
    public string loadoutName;
    public string description;
    public string playerPrefVariable;
    public Sprite icon;
    public bool unlocked;
}
