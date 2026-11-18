using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public Health health;
    public Image[] hearts;
    public TMP_Text coinText;
    public TMP_Text timerText;

    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].enabled = i < health.hp;

        LevelData level = LevelLoader.Instance.Data;

        coinText.text = level.coinsRequired > 0
            ? "Coins: " + GameState.LevelCoins + " / " + level.coinsRequired
            : "Defeat the boss!";

        if (timerText != null)
        {
            int sec = Mathf.CeilToInt(LevelLoader.Instance.TimeLeft);
            timerText.text = (sec / 60) + ":" + (sec % 60).ToString("00");
        }
    }
}