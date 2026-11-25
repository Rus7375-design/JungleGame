using UnityEngine;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;  
    [SerializeField] Slider sfxSlider;   
    [SerializeField] GameObject confirmPanel;
    [SerializeField] Button backButton;
    [SerializeField] Button resetButton;
    [SerializeField] Button yesButton;
    [SerializeField] Button cancelButton;
    const float saveDelay = 0.5f;

    void Awake()
    {
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxChanged);
        backButton.onClick.AddListener(Close);
        resetButton.onClick.AddListener(AskReset);
        yesButton.onClick.AddListener(ConfirmReset);
        cancelButton.onClick.AddListener(CancelReset);
    }

    void OnEnable()
    {
        confirmPanel.SetActive(false);
        ShowSaved();
    }

    void ShowSaved()
    {
        volumeSlider.SetValueWithoutNotify(SaveManager.Instance.Data.musicVol);
        sfxSlider.SetValueWithoutNotify(SaveManager.Instance.Data.sfxVol);
    }

    void OnVolumeChanged(float value)
    {
        SaveManager.Instance.SetMusicVolume(value);
        SaveLater();
    }

    void OnSfxChanged(float value)
    {
        SaveManager.Instance.SetSfxVolume(value);
        SaveLater();
    }

    void SaveLater()
    {
        CancelInvoke(nameof(SaveNow));
        Invoke(nameof(SaveNow), saveDelay);
    }

    void SaveNow() => SaveManager.Instance.Save();

    void Close()
    {
        SaveManager.Instance.Save();
        gameObject.SetActive(false);
    }

    void AskReset() => confirmPanel.SetActive(true);
    void CancelReset() => confirmPanel.SetActive(false);

    void ConfirmReset()
    {
        SaveSystem.Delete();
        PlayerPrefs.DeleteAll();
        GameState.Reset();
        SaveManager.Instance.ResetToDefaults();
        ShowSaved();
        confirmPanel.SetActive(false);
    }
}