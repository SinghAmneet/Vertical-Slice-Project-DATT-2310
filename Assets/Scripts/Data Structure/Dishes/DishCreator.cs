using System.Collections.Generic;
using UnityEngine;

public class DishCreator : MonoBehaviour
{
    private List<Dish> dishEvals;

    public void AddDishEval(Dish dish)
    {
        dishEvals.Add(dish);
    }

    public void GetDish(List<FoodData> foods)
    {
        foreach (var eval in dishEvals)
        {
            bool result = eval.Evaluate(foods);
            if (result)
            {
                Debug.Log("cooked up: " + eval.dishName);
                return;
            }
        }
    }
}
