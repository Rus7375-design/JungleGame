using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Hazard[] prefabs;           
    public GameObject warningPrefab;   
    public float interval = 10f;     
    public float warnTime = 3f;        
    public float groundY = -2f;        

    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(BombLoop());
    }

    IEnumerator BombLoop()
    {
        yield return new WaitForSeconds(2f);   

        while (true)
        {
            if (player == null) yield break;

            float x = player.position.x;     
            Hazard pick = prefabs[Random.Range(0, prefabs.Length)];

            GameObject warn = Instantiate(warningPrefab, new Vector3(x, groundY, 0f), Quaternion.identity);
            SpriteRenderer sr = warn.GetComponent<SpriteRenderer>();

            float t = 0f;
            while (t < warnTime)
            {
                t += Time.deltaTime;
                float k = t / warnTime;                       
                Color c = sr.color;
                c.a = Mathf.Lerp(0.15f, 0.9f, k);              
                sr.color = c;
                yield return null;
            }

            Destroy(warn);
            Instantiate(pick, new Vector3(x, 10f, 0f), Quaternion.identity);   

            yield return new WaitForSeconds(interval - warnTime);   
        }
    }
}