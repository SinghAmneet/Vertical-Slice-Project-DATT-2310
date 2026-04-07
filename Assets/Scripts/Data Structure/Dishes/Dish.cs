using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Items/Dish")]
public class Dish : ScriptableObject
{
    public string dishName;
    public List<FoodData> ingredients;
    protected int inventorySlots = 6;

    public List<FoodData> CloneList()
    {
        List<FoodData> clonedList = new();
        foreach (FoodData foodData in ingredients) clonedList.Add(foodData);
        return clonedList;
    }

    public List<FoodData> GetMissingIngredients(List<FoodData> foods)
    {
        List<FoodData> requiredIngredients = CloneList();

        foreach (FoodData food in foods)
        {
            if (requiredIngredients.Contains(food)) requiredIngredients.Remove(food);
        }
        return requiredIngredients;
    }
}
