using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject credits;

    public void goToIntro()
    {
        if (GameData.currentDay == 0)
        {
            SceneManager.LoadScene("IntroDialogue");
        } else
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    public void toggleCredits()
    {
        bool show  = !credits.activeSelf;
        credits.SetActive(show);
        mainMenu.SetActive(!show);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
