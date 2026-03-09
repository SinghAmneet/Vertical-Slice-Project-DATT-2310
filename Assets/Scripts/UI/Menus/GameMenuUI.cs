using UnityEngine;

public class MainUI : MonoBehaviour
{
    public GameObject htpMenu;
    public GameObject bottomElements;
    public GameObject dishObjective;
    public GameObject timeUI;

    //public GameObject

    void Start()
    {
        Time.timeScale = 0;
        toggleHTP();
    }

    public void toggleDishObjective()
    {
        bool show = !dishObjective.activeSelf;
        dishObjective.SetActive(show);
    }

    public void toggleHTP()
    {
        bool show = !htpMenu.activeSelf;
        htpMenu.SetActive(show);
        dishObjective.SetActive(!show);
        timeUI.SetActive(!show);

        foreach (Transform menu in bottomElements.transform)
        {
            menu.gameObject.SetActive(!show);
        }

        if (!show) Time.timeScale = 1.0f;
    }
}
