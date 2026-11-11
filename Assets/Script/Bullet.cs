using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12f;

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.right * speed;
        Destroy(gameObject, 3f);  
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