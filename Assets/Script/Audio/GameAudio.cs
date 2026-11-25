using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameAudio : MonoBehaviour
{
    public static GameAudio Instance { get; private set; }
    public static SoundBank Bank => Instance.bank;

    [Header("Data")]
    public SoundBank bank;
    public AudioMixer mixer;

    AudioMixerSnapshot normalSnap, pausedSnap;

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
        gameObject.AddComponent<MusicManager>().Init(mixer);
        mixer.updateMode = AudioMixerUpdateMode.UnscaledTime;  
        normalSnap = mixer.FindSnapshot("Normal");
        pausedSnap = mixer.FindSnapshot("Paused");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        if (Instance != this) return;
        ApplyVolumes();                                  
        SaveManager.Instance.VolumeChanged += ApplyVolumes;
    }

    void ApplyVolumes()
    {
        SaveData d = SaveManager.Instance.Data;
        mixer.SetFloat("MusicVol", ToDb(d.musicVol));
        mixer.SetFloat("SFXVol", ToDb(d.sfxVol));
    }

    public static float ToDb(float v) => Mathf.Log10(Mathf.Max(v, 0.0001f)) * 20f;

    void OnDestroy()
    {
        if (Instance != this) return;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        if (SaveManager.Instance != null) SaveManager.Instance.VolumeChanged -= ApplyVolumes;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (Button b in buttons)
            b.onClick.AddListener(PlayClick);
    }

    void PlayClick()
    {
        PlayUi(bank.uiClick);
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

    public static void SetPaused(bool paused)
    {
        if (Instance == null) return;
        (paused ? Instance.pausedSnap : Instance.normalSnap).TransitionTo(0.3f);
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