using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public float cooldown; // time before being able to attack again after an attack

    private float? lastAttacked;
    private bool playingAttackAnimation;

    public Transform attackPoint;
    public float hitboxRadius;

    public float damage;

    private List<GameObject> hitRegistry = new();
    public LayerMask filterLayer;

    public void SetDamage(float damage)
    {
        this.damage = damage;   
    }

    public float GetDistFromPoint()
    {
        return Vector2.Distance(transform.position, attackPoint.position);
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
        hitRegistry.Clear();
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

            if (hitRegistry.Contains(character)) return;
            hitRegistry.Add(character);

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
