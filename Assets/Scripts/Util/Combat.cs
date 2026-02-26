using UnityEngine;

public class Combat : MonoBehaviour
{
    public float cooldown; // time before being able to attack again after an attack

    private float? lastAttacked;
    private bool playingAttackAnimation;

    public Transform attackPoint;
    public float hitboxRadius;

    public float damage;

    public LayerMask filterLayer;

    public void SetDamage(float damage)
    {
        this.damage = damage;   
    }

    // if animation not playing, and if attacking for the first time, or time after attacking is more than the cooldown
    public bool CanAttack()
    {
        return (!playingAttackAnimation && (lastAttacked == null || Time.time - lastAttacked > cooldown) );
    }

    // attack animation playing
    public void Attack()
    {
        playingAttackAnimation = true;
    }

    // attack animation ended
    public void AttackEnd()
    {
        lastAttacked = Time.time;
        playingAttackAnimation = false;
    }

    public void RegisterHits(float damage)
    {
        // get objects in the filtered layer within range
        Collider2D[] hitList = Physics2D.OverlapCircleAll(attackPoint.position, hitboxRadius, filterLayer);

        foreach (Collider2D hit in hitList)
        {
            // get health component and deplete health
            // note: hitboxes will usually be its own separate object under the character
            GameObject character = hit.transform.parent.gameObject;
            character.GetComponent<Health>().Deplete(damage);
            //Debug.Log($"did {damage} to {character.name} who has {character.GetComponent<Health>().GetHealth()} hp left");
        }
    }

    public void RegisterEvent()
    {
        RegisterHits(damage);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, hitboxRadius);
    }
}
