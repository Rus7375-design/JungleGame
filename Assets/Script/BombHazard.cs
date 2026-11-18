using UnityEngine;

public class BombHazard : Hazard
{
    public GameObject explosionPrefab;
    public AudioClip explosionClip;
    bool armed;

    protected override void OnHitPlayer(Health p)
    {
        p.TakeDamage(stats.damage);
        Explode();
    }

    protected override void OnHitGround()
    {
        if (armed) return;
        armed = true;
        Invoke(nameof(Explode), stats.fuse);
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