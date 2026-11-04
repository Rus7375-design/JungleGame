using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public AudioClip clip;
    public bool loop = true;
    [Range(0f, 1f)] public float volume = 0.3f;

    void Start()
    {
        AudioSource s = gameObject.AddComponent<AudioSource>();
        s.clip = clip;
        s.loop = loop;
        s.volume = volume;
        s.Play();
    }
}