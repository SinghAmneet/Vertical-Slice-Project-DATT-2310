using System.Collections.Generic;
using UnityEngine;

// Enums for food stats
public enum Stats
{
    Sweet,
    Salty,
    Sour,
    Bitter,
    Spicy,
}

[CreateAssetMenu(menuName = "Items/Food")]
public class FoodData : ItemData
{
    public Stats[] stats; // list of stats the food will have

    // when the player consumes the food
    public override void Use(GameObject plr)
    {
        
    }
}

