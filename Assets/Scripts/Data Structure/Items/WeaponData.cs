using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon")]
public class WeaponData : ItemData
{
    public float damage; // how much damage per swing does
    public float range; // how far the weapon's hitbox can reach

    // when the player attacks with the weapon
    public override void Use(GameObject plr)
    {

    }
}
