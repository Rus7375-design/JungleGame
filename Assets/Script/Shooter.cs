using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public PlayerStats stats;
    public AudioClip shootClip;
    float nextShot;
    SpriteRenderer sr;
    AudioSource src;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        src = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        if (Time.time < nextShot) return;
        nextShot = Time.time + stats.shootCooldown;

        bool faceRight = sr.flipX;
        Quaternion rot = faceRight ? Quaternion.identity : Quaternion.Euler(0, 0, 180);

        Instantiate(bulletPrefab, transform.position, rot);
        src.PlayOneShot(shootClip);
    }

    bool OnScreen()
    {
        Vector3 v = Camera.main.WorldToViewportPoint(transform.position);
        return v.x > 0.02f && v.x < 0.98f && v.y > 0.02f && v.y < 0.98f;
    }
}