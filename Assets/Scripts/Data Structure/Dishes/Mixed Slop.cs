using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dish/Mixed Slop")]
public class MixedSlop : Dish
{
    public override bool Evaluate(List<FoodData> foods) {  return true; }
}
