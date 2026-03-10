using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dish : ScriptableObject
{
    public string dishName;
    [TextArea]
    public string recipeDescription;
    protected int inventorySlots = 6;

    public bool TypeEquals(FoodData food, string type)
    {
        return food.type.ToString().Equals(type);
    }

    public bool IsHalfOfInventory(int amount)
    {
        return amount == inventorySlots / 2;
    }

    public bool IsEntireInventory(int amount)
    {
        return amount == inventorySlots;
    }

    public int GetNameCount(List<FoodData> foods, string foodName)
    {
        int count = 0;
        foreach (FoodData food in foods)
        {
            if (food.name.Equals(foodName)) count++;
        }
        return count;
    }

    public abstract bool Evaluate(List<FoodData> foods);
}
