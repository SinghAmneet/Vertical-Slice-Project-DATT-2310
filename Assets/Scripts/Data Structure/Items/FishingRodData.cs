using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Fishing Rod")]
public class FishingRodData : ItemData
{
    public float range; // how close the player has to be to water to use the fishing rod

    // when the player uses the fishing rod
    public override void Use(GameObject plr)
    {

    }
}
