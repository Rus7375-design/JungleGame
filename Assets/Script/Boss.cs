using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public int hp = 10;
    public Image hpBar;
    int maxHp;

    void Start()
    {
        maxHp = hp;
    }

    public void TakeHit()
    {
        hp--;

        if (hpBar != null)
        {
            hpBar.fillAmount = (float)hp / maxHp;
        }

        if (hp <= 0)
        {
            SceneManager.LoadScene("Win");
        }
    }
}