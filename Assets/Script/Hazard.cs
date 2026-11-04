using UnityEngine;

public class Hazard : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 10f);   
    }

    void Update()
    {
        if (transform.position.y < -12f) Destroy(gameObject);   
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
        player.TakeDamage(1);
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