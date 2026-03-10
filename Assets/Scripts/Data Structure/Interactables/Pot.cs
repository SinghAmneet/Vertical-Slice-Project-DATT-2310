using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Pot : Interactable
{
    public DishCreator dishCreator;
    public GameObject potUI;
    public Inventory inv;

    public void togglePotUI(bool show)
    {
        potUI.SetActive(show);
    }

    public override void Use(GameObject plr)
    {
        togglePotUI(true);
    }

    public void CreateDish()
    {
        if (inv.GetItems().Count < 2) return;
        // convert to food class
        List<FoodData> foodItems = inv.GetItems().ConvertAll(item => (FoodData) item.data);

        Dish createdDish = dishCreator.GetDish(foodItems);
        Debug.Log(createdDish.ToString());
    }

    public void GoToRhythm()
    {
        CreateDish();
        //SceneManager.LoadScene("RhythmV2");
    }

}
