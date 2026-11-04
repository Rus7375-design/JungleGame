using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public GameObject bulletPrefab;   
    public float range = 14f;           
    public float cooldown = 1.5f;
    public int bulletsPerVolley = 3;    
    public float spread = 15f;         
    public int contactDamage = 1;

    float nextShot;
    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;
        if (Vector2.Distance(player.position, transform.position) > range) return;
        if (Time.time < nextShot) return;
        nextShot = Time.time + cooldown;

        Vector2 dir = player.position - transform.position;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < bulletsPerVolley; i++)
        {
            float offset = (i - (bulletsPerVolley - 1) / 2f) * spread;
            Quaternion rot = Quaternion.Euler(0, 0, baseAngle + offset);
            Instantiate(bulletPrefab, transform.position, rot);
        }
    }
    void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player"))
            c.gameObject.GetComponent<Health>().TakeDamage(contactDamage);
    }
}