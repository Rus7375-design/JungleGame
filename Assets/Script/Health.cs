using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public int hp = 3;
    public int maxHp = 3;
    public AudioClip hurtClip;
    public AudioClip healClip;

    bool invincible;
    SpriteRenderer sr;
    AudioSource src;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        src = GetComponent<AudioSource>();
        GameState.BeginLevel(SceneManager.GetActiveScene().name, maxHp);
        hp = GameState.hp;
    }

    public void TakeDamage(int dmg)
    {
        if (invincible) return;       

        hp -= dmg;
        GameState.hp = hp;
        src.PlayOneShot(hurtClip);

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
        src.PlayOneShot(healClip);
    }

    IEnumerator Invincibility()
    {
        invincible = true;
        for (int i = 0; i < 10; i++)              
        {
            sr.enabled = !sr.enabled;       
            yield return new WaitForSeconds(0.2f); 
        }
        sr.enabled = true;
        invincible = false;
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}