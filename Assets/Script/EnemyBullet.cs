using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public ProjectileStats stats; 

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * stats.speed;
        Destroy(gameObject, stats.lifetime);
    }   

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>().TakeDamage(stats.damage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}