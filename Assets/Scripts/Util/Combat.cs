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

    public GameObject target;
    public Transform rootPoint;

    public void SetDamage(float damage)
    {
        this.damage = damage;   
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public float GetDistFromPoint()
    {
        return Vector2.Distance(rootPoint.position, attackPoint.position);
    }

    public Vector2 GetDirFromTarget()
    {
        return ( (Vector2) target.transform.position + (Vector2.up * 3.5f) - (Vector2) rootPoint.position).normalized;
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

    public Vector2 GetHitboxPos()
    {
        if (target != null)
        {
            return (Vector2)rootPoint.position + GetDirFromTarget() * GetDistFromPoint() ;
        } else
        {
            return (Vector2) attackPoint.position;
        }
    }


    public void RegisterHits(float damage)
    {
        // get objects in the filtered layer within range
        Collider2D[] hitList = Physics2D.OverlapCircleAll(GetHitboxPos(), hitboxRadius, filterLayer);

        foreach (Collider2D hit in hitList)
        {
            // get health component and deplete health
            // note: hitboxes will usually be its own separate object under the character
            GameObject character = hit.transform.parent.gameObject;
            if (hitRegistry.Contains(character)) return;
            hitRegistry.Add(character);

            PlayerHealth playerHealth = character.GetComponent<PlayerHealth>();
            if (playerHealth != null) { playerHealth.Deplete(damage); continue; }

            Health health = character.GetComponent<Health>();
            if (health != null) { health.Deplete(damage); }
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
