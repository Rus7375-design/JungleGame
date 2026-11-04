using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed = 2f;        
    public float distance = 1f;     
    Vector3 start;
    int dir = 1;                  
    SpriteRenderer sr;

    void Start()
    {
        start = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.Translate(Vector3.right * dir * speed * Time.deltaTime);

        if (Mathf.Abs(transform.position.x - start.x) >= distance)
            dir = -dir;                       

        sr.flipX = dir > 0;                   
    }
}