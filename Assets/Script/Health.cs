using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public PlayerStats stats;
    public int hp;
    public int maxHp => stats.maxHp;

    bool invincible;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        GameState.BeginLevel(SceneManager.GetActiveScene().name, maxHp);
        hp = GameState.hp < 0 ? maxHp : GameState.hp;
    }

    public void TakeDamage(int dmg)
    {
        if (invincible) return;

        hp -= dmg;
        GameState.hp = hp;
        GameAudio.Play(GameAudio.Bank.hurt);

        if (hp <= 0)
        {
            GameOver();
            return;
        }
        StartCoroutine(Invincibility());
    }

    public void Heal(int amount)
    {
        hp = Mathf.Min(hp + amount, maxHp);
        GameState.hp = hp;
        GameAudio.Play(GameAudio.Bank.pickup);
    }

    IEnumerator Invincibility()
    {
        invincible = true;
        for (int i = 0; i < stats.blinkCount; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(stats.blinkInterval);
        }
        sr.enabled = true;
        invincible = false;
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}