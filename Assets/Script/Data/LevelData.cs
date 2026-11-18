using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Identity")]
    public int levelNumber = 1;        
    public string levelName = "Level 1";
    public string sceneName = "Level1";  

    [Header("Rules")]
    [Min(1f)] public float timeLimit = 90f;  
    [Min(0)] public int coinsRequired = 3;   
    [Min(0)] public int scorePerCoin = 100;

    [Header("Bombs (interval 0 = no bombs)")]
    [Min(0f)] public float bombInterval = 0f;
    public float bombWarnTime = 3f;
    public float bombStartDelay = 2f;
    public float bombGroundY = -2f;
    public float bombDropHeight = 10f;
}