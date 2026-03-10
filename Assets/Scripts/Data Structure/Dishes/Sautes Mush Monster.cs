using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Dish/Sautes Mush Monster")]
public class SautesMushMonster : Dish
{
    public override bool Evaluate(List<FoodData> foods)
    {
        if (foods.Count == inventorySlots)
        {
            int foundMushrooms = GetNameCount(foods, "Mushroom");
            return IsEntireInventory(foundMushrooms);
        }
        return false;
    }
}
