using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dish/Mixed Vegetables")]
public class MixedVegetables : Dish
{
    public override bool Evaluate(List<FoodData> foods)
    {
        int differentVegetables = 0;
        int vegetables = 0;

        foreach (FoodData food in foods)
        {
            if (TypeEquals(food, "Vegetable"))
            {
                vegetables++;
                //Debug.Log(food.ToString());
                bool foundSimilarFood = false;

                foreach (FoodData otherFood in foods)
                {
                    if (food != otherFood && food.Equals(otherFood))
                    {
                        foundSimilarFood = true;
                        break;
                    }
                }

                if (!foundSimilarFood) differentVegetables++;
            };
        }
        //Debug.Log(vegetables);
        //Debug.Log(differentVegetables);
        if (vegetables != foods.Count) return false; // not all of the foods are vegetables

        return differentVegetables >= 3; // found 3 or more different types of 

    }
}
