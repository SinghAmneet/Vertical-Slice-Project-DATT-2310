using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName ="Mob")]
public class MobData : ScriptableObject
{
    [Header("Looks")]
    public Sprite sprite;
    public AnimatorController animatorController;

    [Header("Food Drop")]
    public int dropAmount = 1; // amount to drop
    public FoodData[] foodDrops; // the foods the mob can drop

    [Header("Combat")]
    public float maxHp;
    public float damage; // how much hp the mob will deal each attack
    public float attackCooldown; // time before the mob can attack again after an attack

    [Header("Ranges")]
    public float chaseRange; // if the player is out of this range, the mob will stop chasing
    public int attackRange; // when the player is within this range the mob will stop and attack

    [Tooltip("the radius of the attack hitbox from the mob's attack point")]
    public float hitboxRadius = 0.5f;

    [Header("Movement")]
    public float speed;
}
