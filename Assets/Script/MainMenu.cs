using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        GameState.Reset();
        SceneManager.LoadScene("Level1");   
    }

    public void Quit()
    {
        Application.Quit();
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}