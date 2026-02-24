using UnityEngine;

[CreateAssetMenu(menuName ="Mob")]
public class MobData : ScriptableObject
{
    [Header("Looks")]
    public Sprite sprite;
    public GameObject mobPrefab;

    [Header("Food Drop")]
    public int dropAmount = 1;
    public FoodData[] foodDrops;

    [Header("Combat")]
    public float maxHp;
    public float damage;
    public float attackCooldown;

    [Header("Ranges")]
    public float chaseRange;
    public int attackRange;

    [Tooltip("the radius of the attack hitbox from the mob's attack point")]
    public float hitboxRadius = 0.5f;

    [Header("Movement")]
    public float speed;
}