using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public Health health;
    public Image[] hearts;      
    public TMP_Text coinText;
    public int totalCoins = 7;

    void Update()
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].enabled = i < health.hp;     

        coinText.text = "Coins: " + GameState.coins + " / " + totalCoins;
    }
}