using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public PlayerStats stats;
    float nextShot;
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Shoot()
    {
        if (Time.time < nextShot) return;
        nextShot = Time.time + stats.shootCooldown;

        bool faceRight = sr.flipX;
        Quaternion rot = faceRight ? Quaternion.identity : Quaternion.Euler(0, 0, 180);

        Instantiate(bulletPrefab, transform.position, rot);
        GameAudio.Play(GameAudio.Bank.shoot);
    }
}