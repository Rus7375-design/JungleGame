using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public const int CurrentVersion = 1; 

    public int version = CurrentVersion;
    public int bestScore;
    public List<int> unlockedLevels = new List<int> { 1 };   
    public float musicVol = 1f;       
    public float sfxVol = 1f;                       

    public void Validate()
    {
        if (unlockedLevels == null) unlockedLevels = new List<int>();
        if (!unlockedLevels.Contains(1)) unlockedLevels.Add(1);
        bestScore = Mathf.Max(0, bestScore);
        musicVol = Mathf.Clamp01(musicVol);
        sfxVol = Mathf.Clamp01(sfxVol);
    }
}