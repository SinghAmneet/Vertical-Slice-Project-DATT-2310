using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject credits;

    public void goToIntro()
    {
        SceneManager.LoadScene("IntroDialogue");
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
