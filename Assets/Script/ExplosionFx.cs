using UnityEngine;

public class ExplosionFx : MonoBehaviour
{
    public HazardStats stats;

    void Start()
    {
        Destroy(gameObject, stats.explosionLifetime);
    }

    void Update()
    {
        transform.localScale += Vector3.one * stats.explosionGrow * Time.deltaTime;
    }
}