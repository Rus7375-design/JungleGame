using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public BossStats stats;
    public Image hpBar;
    int hp;
    bool dead;

    void Start()
    {
        hp = stats.maxHp;
    }

    public void TakeHit()
    {
        if (dead) return;

        hp--;

        if (hpBar != null)
            hpBar.fillAmount = (float)hp / stats.maxHp;

        if (hp <= 0)
        {
            dead = true;
            SaveManager.Instance.LevelCompleted(LevelLoader.Instance.Data);
            GameAudio.Play(GameAudio.Bank.win, vary: false);
            SceneManager.LoadScene("Win");
        }
    }
}