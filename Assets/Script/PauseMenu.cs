using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    void Start()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        GameAudio.SetPaused(false);
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        GameAudio.SetPaused(true);
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        GameAudio.SetPaused(false);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        GameAudio.SetPaused(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        GameAudio.SetPaused(false);
        SceneManager.LoadScene("MainMenu");
    }
}