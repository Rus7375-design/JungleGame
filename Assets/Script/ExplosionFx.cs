using UnityEngine;

public class ExplosionFx : MonoBehaviour
{
    public float lifetime = 0.4f;
    public float grow = 10f;  

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.localScale += Vector3.one * grow * Time.deltaTime;
    }
}