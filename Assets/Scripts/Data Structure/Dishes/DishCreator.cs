using System.Collections.Generic;
using UnityEngine;

public class DishCreator : MonoBehaviour
{
    public List<Dish> dishEvals;
    public Dish slop;

    public Dish GetDish(List<FoodData> foods)
    {
        foreach (Dish dish in dishEvals)
        {
            bool result = dish.Evaluate(foods);
            if (result)
            {
                Debug.Log("cooked up: " + dish.dishName);
                return dish;
            }
        }
        Debug.Log("cooked up: slop");
        return slop;
    }
}
