using UnityEngine;

[CreateAssetMenu(fileName = "AllLevels", menuName = "Levels", order = 1)]
public class Levels : ScriptableObject
{
    public Level[] levels;
}
