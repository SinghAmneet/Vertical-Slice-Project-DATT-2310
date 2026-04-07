using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class Pot : Interactable
{
    public GameObject potUI;
    public TextMeshProUGUI missingIngredientText;
    public Inventory inv;

    private bool canCook;

    public void togglePotUI(bool show)
    {
        potUI.SetActive(show);
    }

    public override void Use(GameObject plr)
    {
        togglePotUI(true);
        CheckIngredients();
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

    public void CheckIngredients()
    {
        // convert to food class
        List<FoodData> foodItems = inv.GetItems().ConvertAll(item => (FoodData) item.data);

        List<FoodData> missingIngredients = GameData.currentDish.GetMissingIngredients(foodItems);
        canCook = missingIngredients.Count == 0;

        if (canCook)
        {
            missingIngredientText.text = "you have all the ingredients!";
        } else
        {
            missingIngredientText.text = "missing ingredients: ";
            for (int i = 0; i < missingIngredients.Count; i++)
            {
                missingIngredientText.text += missingIngredients[i].name;
                if (i < missingIngredients.Count - 1) missingIngredientText.text += ", ";
            }
        }
    }

    public void GoToRhythm()
    {
        if (!canCook) return;
        // convert to food class
        List<FoodData> foodItems = inv.GetItems().ConvertAll(item => (FoodData)item.data);
        addStats(foodItems);
        DishData.createdDish = GameData.currentDish;
        DishData.inventory = foodItems;

        GameData.loadedDialogue = false;
        SceneLoader.LoadScene("RhythmV2");
        //SceneManager.LoadScene("RhythmV2");
    }

}
