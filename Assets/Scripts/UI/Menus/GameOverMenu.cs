using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject dialogueEndMenu;
    public GameObject deathEndMenu;
    public GameObject executionEndMenu;

    void Start()
    {
        deathEndMenu.SetActive(false);
        dialogueEndMenu.SetActive(false);
        executionEndMenu.SetActive(false);

        if (EndingData.currentEnding == endings.Death)
        {
            deathEndMenu.SetActive(true);
        } else if (EndingData.currentEnding == endings.WrongDialogue)
        {
            dialogueEndMenu.SetActive(true);
        } else
        {
            executionEndMenu.SetActive(true);
        }
        
    }

    public void MainMenu()
    {
        EndingData.currentEnding = endings.None;
        RhythmProgressData.ResetGameProgress();     // Reseting rhythm game difficulty back to easy
        SceneManager.LoadScene("StartMenu");

    }

    public void Restart()
    {

        if(RhythmProgressData.rhythmRoundIndex != 0) RhythmProgressData.rhythmRoundIndex -= 1; // TO go back a difficulty in rhythm game
        EndingData.currentEnding = endings.BadDish;
        SceneManager.LoadScene("MainScene");

    }

    public void RestartChoice()
    {
        SceneManager.LoadScene("IntroDialogue");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
