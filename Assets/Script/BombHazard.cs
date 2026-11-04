using UnityEngine;

public class BombHazard : Hazard
{
    public GameObject explosionPrefab;
    public float fuse = 2f;
    bool armed;
    public AudioClip explosionClip;

    protected override void OnHitPlayer(Health p)
    {
        p.TakeDamage(2);
        Explode();                  
    }

    protected override void OnHitGround()
    {
        if (armed) return;           
        armed = true;
        Invoke(nameof(Explode), fuse); 
    }

    public override void OnShot()
    {
        Explode();              
    }

    void Explode()
    {
        CancelInvoke();               
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionClip, transform.position);
        Destroy(gameObject);
    }
}