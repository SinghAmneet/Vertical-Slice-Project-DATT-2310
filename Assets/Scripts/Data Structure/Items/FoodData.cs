using System.Collections.Generic;
using UnityEngine;

public enum Types
{
    Vegetable,
    Fruit,
    Meat,
    Grain,
    Dairy,
    Other
}

[CreateAssetMenu(menuName = "Items/Food")]
public class FoodData : ItemData
{
    public Types type;
    public List<Stat> stats = new();

    // when the player consumes the food
    public override void Use(GameObject plr)
    {
        
    }
}

