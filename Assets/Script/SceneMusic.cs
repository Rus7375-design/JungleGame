using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public AudioClip clip;

    void Start()
    {
        if (MusicManager.Instance != null) MusicManager.Instance.Play(clip);
    }
}