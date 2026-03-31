using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dish/Basic Dish")]
public class BasicDish : Dish
{
    public override bool Evaluate(List<FoodData> foods)
    {
        if (foods.Count == ingredients.Count)
        {
            return InvHasIngredients(foods);
        }

        return false;
    }
}
