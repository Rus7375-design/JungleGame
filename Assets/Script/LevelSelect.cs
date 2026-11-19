using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] LevelData[] levels;    
    [SerializeField] Button[] buttons;     
    [SerializeField] TMP_Text bestScoreText;

    void OnEnable()
    {
        SaveManager save = SaveManager.Instance;

        for (int i = 0; i < levels.Length; i++)
        {
            bool open = save.IsUnlocked(levels[i].levelNumber);
            buttons[i].interactable = open;
            buttons[i].GetComponentInChildren<TMP_Text>().text =
                open ? levels[i].levelName : levels[i].levelName + " (locked)";
        }

        bestScoreText.text = "Best score: " + save.Data.bestScore;
    }

    public void Play(int index)
    {
        LevelData level = levels[index];
        if (!SaveManager.Instance.IsUnlocked(level.levelNumber)) return;

        GameState.Reset();
        GameSession.levelIndex = index;
        SceneManager.LoadScene(level.sceneName);
    }
}