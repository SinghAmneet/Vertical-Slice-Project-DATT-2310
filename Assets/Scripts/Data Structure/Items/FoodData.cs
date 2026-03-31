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

    public float healAmount;

    private List<Color> colors = new()
    {
        Color.green,
        Color.blue,
        Color.red,
        Color.yellow,
        Color.white,
        Color.gray,
    };

    public bool Equals(FoodData other)
    {
        return this.name.Equals(other.name);
    }

    public Color GetColor()
    {
        return colors[(int) type];
    }

    // when the player consumes the food
    public override void Use(GameObject plr)
    {
        Health health = plr.GetComponent<Health>();
        if (this.healAmount > 0)
        {
            health.Heal(this.healAmount);
        } else
        {
            health.Deplete(this.healAmount);
        }

    }
}

