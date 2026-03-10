using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public GameObject dialogueEndMenu;
    public GameObject deathEndMenu;

    void Start()
    {
        if (EndingData.currentEnding == endings.Death)
        {
            deathEndMenu.SetActive(true);
            dialogueEndMenu.SetActive(false);
        } else
        {
            deathEndMenu.SetActive(false);
            dialogueEndMenu.SetActive(true);
        }
        
    }

    public void MainMenu()
    {
        EndingData.currentEnding = endings.None;
        SceneManager.LoadScene("StartMenu");
    }

    public void Restart()
    {
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
