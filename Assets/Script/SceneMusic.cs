using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public AudioClip clip;
    public bool loop = true;
    [Range(0f, 1f)] public float volume = 0.3f;  

    AudioSource source;

    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = loop;
        Apply();
        source.Play();
        SaveManager.Instance.VolumeChanged += Apply;
    }

    void OnDestroy()
    {
        if (SaveManager.Instance != null)
            SaveManager.Instance.VolumeChanged -= Apply;
    }

    void Apply()
    {
        source.volume = volume * SaveManager.Instance.Data.musicVol;
    }
}