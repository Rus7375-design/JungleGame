using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public BossStats stats;
    public Image hpBar;
    int hp;  

    void Start()
    {
        hp = stats.maxHp;
    }

    public void TakeHit()
    {
        hp--;

        if (hpBar != null)
            hpBar.fillAmount = (float)hp / stats.maxHp;

        if (hp <= 0)
            SceneManager.LoadScene("Win");
    }
}