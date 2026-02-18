using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Food")]
public class FoodData : ItemData
{
    public Stats[] stats; // list of stats the food will have

    // when the player consumes the food
    public override void Use(GameObject plr)
    {
        
    }
}

// Enums for food stats
public enum Stats
{
    Sweet,
    Sour,
    Bitter,
    Salty,
    Umami
}