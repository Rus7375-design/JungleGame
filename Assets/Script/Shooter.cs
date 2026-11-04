using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float cooldown = 0.3f;
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
        nextShot = Time.time + cooldown;

        bool faceRight = sr.flipX;
        Quaternion rot = faceRight ? Quaternion.identity : Quaternion.Euler(0, 0, 180);

        Instantiate(bulletPrefab, transform.position, rot);
        src.PlayOneShot(shootClip);
    }
}