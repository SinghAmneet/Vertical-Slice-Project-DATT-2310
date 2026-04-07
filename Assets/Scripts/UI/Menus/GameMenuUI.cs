using UnityEngine;

public class MainUI : MonoBehaviour
{
    public GameObject htpMenu;
    public GameObject bottomElements;
    public GameObject dishObjective;
    public GameObject timeUI;
    public GameObject recipeMenu;
    public GameObject navMap;
    private GameObject controls;

    void Start()
    {
        controls = htpMenu.transform.GetChild(2).gameObject;
        if (GameData.currentDay == 0)
        {
            toggleHTP();
            controls.SetActive(false);
        }
    }

    public void toggleNavMap()
    {
        bool show = !navMap.activeSelf;
        navMap.SetActive(show);
        recipeMenu.SetActive(false);
        dishObjective.SetActive(false);
        htpMenu.SetActive(false);

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

        controls.SetActive(true);

        foreach (Transform menu in bottomElements.transform)
        {
            menu.gameObject.SetActive(!show);
        }

        if (!show) Time.timeScale = 1.0f;
    }
}
