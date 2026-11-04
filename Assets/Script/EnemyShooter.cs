using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float range = 6f;        
    public float cooldown = 1.5f; 

    float nextShot;
    Transform player;
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;
        if (Vector2.Distance(player.position, transform.position) > range) return;

        float dx = player.position.x - transform.position.x;
        sr.flipX = dx > 0;                 

        if (Time.time < nextShot) return;
        nextShot = Time.time + cooldown;

        Quaternion rot = dx > 0 ? Quaternion.identity : Quaternion.Euler(0, 0, 180);
        Instantiate(bulletPrefab, transform.position, rot);
    }
}