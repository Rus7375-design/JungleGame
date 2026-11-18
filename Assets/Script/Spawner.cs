using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Hazard[] prefabs;
    public GameObject warningPrefab;

    Transform player;
    LevelData level;

    void Start()
    {
        level = LevelLoader.Instance.Data;
        if (level.bombInterval <= 0f) return;   
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(BombLoop());
    }

    IEnumerator BombLoop()
    {
        yield return new WaitForSeconds(level.bombStartDelay);

        while (true)
        {
            if (player == null) yield break;

            float x = player.position.x;
            Hazard pick = prefabs[Random.Range(0, prefabs.Length)];

            GameObject warn = Instantiate(warningPrefab,
                new Vector3(x, level.bombGroundY, 0f), Quaternion.identity);
            SpriteRenderer sr = warn.GetComponent<SpriteRenderer>();

            float t = 0f;
            while (t < level.bombWarnTime)
            {
                t += Time.deltaTime;
                float k = t / level.bombWarnTime;
                Color c = sr.color;
                c.a = Mathf.Lerp(0.15f, 0.9f, k);
                sr.color = c;
                yield return null;
            }

            Destroy(warn);
            Instantiate(pick, new Vector3(x, level.bombDropHeight, 0f), Quaternion.identity);

            yield return new WaitForSeconds(Mathf.Max(0f, level.bombInterval - level.bombWarnTime));
        }
    }
}