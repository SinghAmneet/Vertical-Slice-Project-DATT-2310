using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Mob")]
public class MobData : ScriptableObject
{
    public Sprite sprite;
    public FoodData foodDrop;

    public float maxHp;
    public float damage;
}
