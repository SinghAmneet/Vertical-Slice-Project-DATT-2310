using UnityEngine;

public class Combat : MonoBehaviour
{
    public float cooldown = 0; // time before being able to attack again after an attack
    private float lastAttacked = -1f;

    public Transform attackPoint;
    public float hitboxRadius;

    public LayerMask filterLayer;

    public void Setup(Transform attackPoint, float hitboxRadius, float cooldown)
    {
        this.attackPoint = attackPoint;
        this.hitboxRadius = hitboxRadius;
        this.cooldown = cooldown;
    }

    // if attacking for the first time, or time after attacking is more than the cooldown
    public bool CanAttack()
    {
        return (lastAttacked == -1 || Time.time - lastAttacked > cooldown);
    }

    // damages objects in the filtered layer within range
    public bool Attack(float damage)
    {
        if (CanAttack())
        {
            // get last attacked time stamp
            lastAttacked = Time.time;

            // get objects in the filtered layer within range
            Collider2D[] hitList = Physics2D.OverlapCircleAll(attackPoint.position, hitboxRadius, filterLayer);

            foreach (Collider2D hit in hitList)
            {
                // get health component and deplete health
                hit.GetComponent<Health>().Deplete(damage);
                Debug.Log("hit: " + hit.name);
            }

            return true;
        }

        return false;
    }
}
