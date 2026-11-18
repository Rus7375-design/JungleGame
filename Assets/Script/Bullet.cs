using UnityEngine;

public class Bullet : MonoBehaviour
{
    public ProjectileStats stats;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * stats.speed;
        Destroy(gameObject, stats.lifetime);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (other.TryGetComponent(out Hazard hazard))
        {
            hazard.OnShot();     
            Destroy(gameObject);
        }
        else if (other.TryGetComponent(out Boss boss))
        {
            boss.TakeHit();
            Destroy(gameObject);
        }
        else if (other.TryGetComponent(out Patrol patrol))
        {
            patrol.OnShot();
            Destroy(gameObject);
        }
        else if (other.TryGetComponent(out EnemyShooter shooter))
        {
            shooter.OnShot();
            Destroy(gameObject);
        }
        else if (other.TryGetComponent(out Charger charger))
        {
            charger.OnShot();
            Destroy(gameObject);
        }

    }
}