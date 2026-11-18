using UnityEngine;

public class Hazard : MonoBehaviour
{
    public HazardStats stats;

    void Start()
    {
        Destroy(gameObject, stats.lifetime);
    }

    void Update()
    {
        if (transform.position.y < stats.killY) Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            OnHitPlayer(c.gameObject.GetComponent<Health>());
            Destroy(gameObject);
        }
        else if (c.gameObject.CompareTag("Ground"))
        {
            OnHitGround();
        }
    }

    protected virtual void OnHitPlayer(Health player)
    {
        player.TakeDamage(stats.damage);
    }

    protected virtual void OnHitGround()
    {
        Destroy(gameObject);
    }

    public virtual void OnShot()
    {
        Destroy(gameObject);
    }
}