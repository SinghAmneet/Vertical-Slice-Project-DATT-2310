using UnityEngine;

public class MainUI : MonoBehaviour
{
    public GameObject htpMenu;
    public GameObject bottomElements;
    public GameObject dishObjective;
    public GameObject timeUI;
    public GameObject recipeMenu;
    public GameObject navMap;

    void Start()
    {
        if (GameData.currentDay == 0)
        {
            //Time.timeScale = 0;
            toggleHTP();
        }
    }

    public void toggleNavMap()
    {
        bool show = !navMap.activeSelf;
        navMap.SetActive(show);
        recipeMenu.SetActive(false);
    }
    public void toggleRecipes()
    {
        bool show = !recipeMenu.activeSelf;
        recipeMenu.SetActive(show);

        dishObjective.SetActive(!show);
        //if (show) dishObjective.SetActive(true);
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
