
using System.Collections.Generic;

public static class DishData
{
    // get dish name using DishData.createdDish.dishName
    public static Dish createdDish; 

    // when looping through with the stat key, convert the key using .toString(), as the stat key is an Enum and not an actual string
    public static Dictionary<Stats, int> totalStats;

    // get the food name using DishData.inventory[i].name
    public static List<FoodData> inventory; 

    /*  inventory note
     * it only stores non empty inventory slots, as cooking only requires minimum two food items
     * this means that inventory can have different lengths, so if you're looping through it using indexes
     * make sure you use the length of the array, and not the actual amount of inventory slots
     */
}
