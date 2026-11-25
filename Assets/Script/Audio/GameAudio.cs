using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance { get; private set; }
    public static SoundBank Bank => Instance.bank;

    [Header("Data")]
    public SoundBank bank;
    public AudioMixer mixer;

    [Header("Sources (each one is routed to its mixer group in the prefab)")]
    [SerializeField] AudioSource sfxVaried;  
    [SerializeField] AudioSource sfxFixed;    
    [SerializeField] AudioSource ui;          

    [Header("Variation")]
    [SerializeField] float pitchMin = 0.92f;
    [SerializeField] float pitchMax = 1.08f;

    readonly Dictionary<AudioClip[], int> lastIndex = new Dictionary<AudioClip[], int>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Boot()
    {
        GameAudio prefab = Resources.Load<GameAudio>("GameAudio");
        if (prefab == null)
        {
            Debug.LogError("Prefab 'GameAudio' is missing in Assets/Resources");
            return;
        }
        Instantiate(prefab);
    }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }   
        Instance = this;
        DontDestroyOnLoad(gameObject);                           
    }

    public static void Play(AudioClip clip, bool vary = true, float volume = 1f)
    {
        if (Instance == null || clip == null) return;
        Instance.PlayNow(clip, vary, volume);
    }

    public static void Play(AudioClip[] clips, bool vary = true, float volume = 1f)
    {
        if (Instance == null || clips == null || clips.Length == 0) return;
        Instance.PlayNow(Instance.Pick(clips), vary, volume);
    }

    public static void PlayUi(AudioClip clip)
    {
        if (Instance == null || clip == null) return;
        Instance.ui.PlayOneShot(clip);
    }

    void PlayNow(AudioClip clip, bool vary, float volume)
    {
        if (vary)
        {
            sfxVaried.pitch = Random.Range(pitchMin, pitchMax);
            sfxVaried.PlayOneShot(clip, volume);
        }
        else
        {
            sfxFixed.PlayOneShot(clip, volume);
        }
    }

    AudioClip Pick(AudioClip[] clips)
    {
        int i = Random.Range(0, clips.Length);  
        if (clips.Length > 1 && lastIndex.TryGetValue(clips, out int last) && i == last)
            i = (i + 1) % clips.Length;
        lastIndex[clips] = i;
        return clips[i];
    }
}