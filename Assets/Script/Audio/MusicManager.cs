using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    AudioSource a, b, current;
    Coroutine fading;

    public void Init(AudioMixer mixer)
    {
        Instance = this;
        AudioMixerGroup tracks = mixer.FindMatchingGroups("Master/Music/Tracks")[0];
        a = MakeSource("MusicA", tracks);
        b = MakeSource("MusicB", tracks);
        current = a;
    }

    AudioSource MakeSource(string name, AudioMixerGroup group)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(transform);
        AudioSource s = go.AddComponent<AudioSource>();
        s.loop = true;
        s.playOnAwake = false;
        s.spatialBlend = 0f;
        s.volume = 0f;
        s.outputAudioMixerGroup = group;
        return s;
    }

    public void Play(AudioClip clip, float fade = 1f)
    {
        if (clip == null) return;
        if (current.clip == clip && current.isPlaying) return;  

        if (fading != null) StopCoroutine(fading);
        AudioSource from = current;
        AudioSource next = current == a ? b : a;
        next.Stop();
        next.clip = clip;
        next.volume = 0f;
        next.Play();
        current = next;
        fading = StartCoroutine(Crossfade(from, next, fade));
    }

    IEnumerator Crossfade(AudioSource from, AudioSource to, float time)
    {
        float fromStart = from.volume;
        for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime / time)
        {
            from.volume = Mathf.Lerp(fromStart, 0f, t);
            to.volume = t;
            yield return null;
        }
        from.Stop();
        from.volume = 0f;
        to.volume = 1f;
        fading = null;
    }
}