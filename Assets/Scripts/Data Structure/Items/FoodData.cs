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
    private Dictionary<Stats, int> statDictionary;

    public Dictionary<Stats, int> GetStats()
    {
        if (statDictionary == null)
        {
            statDictionary = new();
            foreach (var entry in stats)
            {
                statDictionary[entry.stat] = entry.value;
            }
        }
        return statDictionary;
    }

    // when the player consumes the food
    public override void Use(GameObject plr)
    {
        
    }
}

