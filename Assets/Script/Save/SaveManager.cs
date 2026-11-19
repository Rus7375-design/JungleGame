using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public SaveData Data { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Boot()
    {
        GameObject go = new GameObject("SaveManager");
        go.AddComponent<SaveManager>();
        DontDestroyOnLoad(go);
    }

    void Awake()
    {
        Instance = this;
        Data = SaveSystem.Load();
        Debug.Log("Save file: " + SaveSystem.FilePath);
    }

    public void Save() => SaveSystem.Save(Data);

    public bool IsUnlocked(int levelNumber) => Data.unlockedLevels.Contains(levelNumber);

    public void LevelCompleted(LevelData level)
    {
        Data.bestScore = Mathf.Max(Data.bestScore, GameState.score);

        int next = level.levelNumber + 1;
        if (!level.isFinalLevel && !Data.unlockedLevels.Contains(next))
            Data.unlockedLevels.Add(next);

        Save();
    }

    public void ResetToDefaults() => Data = new SaveData();

    void OnApplicationPause(bool paused)
    {
        if (paused) Save();
    }

    [ContextMenu("Save now")]
    void DebugSave() => Save();
}