using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public BossStats stats;
    public GameObject bulletPrefab;

    float nextShot;
    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;
        if (Vector2.Distance(player.position, transform.position) > stats.range) return;
        if (Time.time < nextShot) return;
        nextShot = Time.time + stats.cooldown;

        Vector2 dir = player.position - transform.position;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        GameAudio.Play(GameAudio.Bank.bossShot);

        for (int i = 0; i < stats.bulletsPerVolley; i++)
        {
            float offset = (i - (stats.bulletsPerVolley - 1) / 2f) * stats.spread;
            Quaternion rot = Quaternion.Euler(0, 0, baseAngle + offset);
            Instantiate(bulletPrefab, transform.position, rot);
        }
    }

    void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player"))
            c.gameObject.GetComponent<Health>().TakeDamage(stats.contactDamage);
    }
}