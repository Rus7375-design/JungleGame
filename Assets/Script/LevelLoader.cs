using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    [SerializeField] LevelData data;   

    public LevelData Data => data;
    public float TimeLeft { get; private set; }

    void Awake()
    {
        Instance = this;
        GameSession.levelIndex = data.levelNumber - 1;
        TimeLeft = data.timeLimit;
    }

    void Start()
    {
        int sceneCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        if (sceneCoins < data.coinsRequired)
            Debug.LogWarning(data.name + ": needs " + data.coinsRequired +
                             " coins but the scene has only " + sceneCoins);
    }

    void Update()
    {
        if (TimeLeft > 0f) TimeLeft = Mathf.Max(0f, TimeLeft - Time.deltaTime);
    }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}