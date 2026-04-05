using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Pot : Interactable
{
    public DishCreator dishCreator;
    public Gameloop gameManager;
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

    private void addStats(List<FoodData> foodItems)
    {
        Dictionary<Stats, int> addedStats = new() {
            {Stats.Sweet, 0 },
            {Stats.Salty, 0 },
            {Stats.Sour, 0 },
            {Stats.Bitter, 0 },
            {Stats.Spicy, 0 },
        };

        foreach (FoodData food in foodItems)
        {
            foreach (Stat stat in food.stats)
            {
                addedStats[stat.stat] += stat.value;
            }
        }

        //foreach (var (stat, value) in addedStats)
        //{
        //    Debug.Log(stat.ToString() + " " + value);
        //}

        DishData.totalStats = addedStats;
    }

    public void CreateDish()
    {
        if (inv.GetItems().Count < 2) return;
        // convert to food class
        List<FoodData> foodItems = inv.GetItems().ConvertAll(item => (FoodData) item.data);

        Dish createdDish = dishCreator.GetDish(foodItems);

        //if (createdDish.name.Equals(gameManager.GetCurrentDish().name))
        //{
            addStats(foodItems);
            DishData.createdDish = createdDish;
            DishData.inventory = foodItems;

            GameData.loadedDialogue = false;
            SceneLoader.LoadScene("RhythmV2");
        //}
    }

    public void GoToRhythm()
    {
        CreateDish();
    }

}
