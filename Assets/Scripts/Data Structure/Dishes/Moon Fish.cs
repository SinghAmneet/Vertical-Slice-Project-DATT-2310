using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dish/Moon Fish")]
public class MoonFish : Dish
{
    public override bool Evaluate(List<FoodData> foods)
    {
        int moonFishCount = GetNameCount(foods, "Moon Fish Meat");
        return IsHalfOfInventory(moonFishCount) && foods.Count == moonFishCount; // exactly half inventory is moonfish, and no other foods
    }
}
