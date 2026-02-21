using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName ="Mob")]
public class MobData : ScriptableObject
{
    //public Sprite sprite;
    public AnimatorController animatorController;
    public FoodData foodDrop; // the food the mob will drop

    public float maxHp;
    public float damage; // how much hp the mob will deal each attack
    public float attackCooldown; // time before the mob can attack again after an attack

    public float chaseRange; // if the player is out of this range, the mob will stop chasing
    public int attackRange; // when the player is within this range the mob will stop and attack
    public float hitboxRadius = 0.5f; // the radius of the attack hitbox from the mob's attack point
}
