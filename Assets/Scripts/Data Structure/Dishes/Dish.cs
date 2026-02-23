using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dish : MonoBehaviour
{
    public string dishName;

    protected virtual void Awake()
    {
        GetComponent<DishCreator>().AddDishEval(this);
    }

    public abstract bool Evaluate(List<FoodData> foods);
}
