using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dish/Mixed Mushrooms")]
public class MixedMushrooms : Dish
{
    public override bool Evaluate(List<FoodData> foods)
    {
        if (foods.Count == inventorySlots)
        {
            int mushroomCount = GetNameCount(foods, "Mushroom");
            return IsHalfOfInventory(mushroomCount);
        }
        return false;
    }
}
