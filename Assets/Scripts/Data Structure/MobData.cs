using UnityEngine;

[CreateAssetMenu(menuName ="Mob")]
public class MobData : ScriptableObject
{
    [Header("Looks")]
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

    [Header("Movement")]
    public float speed;
}