using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject dialogueEndMenu;
    public GameObject deathEndMenu;
    public GameObject executionEndMenu;

    void Start()
    {
        if (EndingData.currentEnding == endings.Death)
        {
            deathEndMenu.SetActive(true);
            dialogueEndMenu.SetActive(false);
            executionEndMenu.SetActive(false);
        } else if (EndingData.currentEnding == endings.WrongDialogue)
        {
            deathEndMenu.SetActive(false);
            dialogueEndMenu.SetActive(true);
            executionEndMenu.SetActive(false);
        } else
        {
            executionEndMenu.SetActive(true);
            deathEndMenu.SetActive(false);
            dialogueEndMenu.SetActive(false);
        }
        
    }

    public void MainMenu()
    {
        EndingData.currentEnding = endings.None;
        RhythmProgressData.ResetGameProgress();     // Reseting rhythm game difficulty back to easy
        SceneLoader.LoadScene("StartMenu");
    }

    public void Restart()
    {
        RhythmProgressData.rhythmRoundIndex -= 1;   // TO go back a difficulty in rhythm game
        SceneLoader.LoadScene("MainScene");
    }

    public void RestartChoice()
    {
        SceneLoader.LoadScene("IntroDialogue");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
