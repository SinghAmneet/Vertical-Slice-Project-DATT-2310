using UnityEngine;

public class MainUI : MonoBehaviour
{
    public GameObject htpMenu;
    public GameObject bottomElements;
    public GameObject dishObjective;
    public GameObject timeUI;
    public GameObject recipeMenu;

    void Start()
    {
        Time.timeScale = 0;
        toggleHTP();
    }

    public void toggleRecipes()
    {
        bool show = !recipeMenu.activeSelf;
        recipeMenu.SetActive(show);

        dishObjective.SetActive(false);
    }

    public void toggleDishObjective()
    {
        bool show = !dishObjective.activeSelf;
        dishObjective.SetActive(show);
        recipeMenu.SetActive(false);
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
