using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject levelSelectPanel;

    public void Play()
    {
        levelSelectPanel.SetActive(true);
    }

    public void CloseLevels()
    {
        levelSelectPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public GameObject settingsPanel;

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }
}