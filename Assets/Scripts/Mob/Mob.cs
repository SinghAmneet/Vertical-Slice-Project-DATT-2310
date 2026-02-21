using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum MobState
{
    Roam, // mob roams around the home point
    Idle, // mob is standing still
    ReturningHome, // mob is returning to the home point
    Chase, // mob is chasing player
    Attack, // mob is attacking player
}

public class Mob : MonoBehaviour
{
    private Vector3 homePoint; // the position the mob will always go back to when not chasing
    public int homeRadius; // the radius around the home point which the mob will roam around
    private Vector2 targetPos; // target position while in roam state

    public Transform attackPoint;
    public GameObject itemPrefab;

    private float idleTimer;
    public float maxIdleTime; // max time for standing still

    // systems
    private MobMovement movement;
    private Health health;
    private Combat combat; 

    private Animator animator;
    private GameObject plr; // when player is set, it will be assumed that the mob is chasing them

    private MobData data;
    private MobState state = MobState.Roam;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        combat = GetComponent<Combat>();
        homePoint = transform.position;
        StartRoam();
    }

    public void SetData(MobData data)
    {
        this.data = data;
        health.SetMaxHealth(data.maxHp);
        combat.Setup(attackPoint, data.hitboxRadius, data.attackCooldown);

        health.OnDeath += Die; // call Die() method when health is 0
    }
    
    public void SetHomePoint(Vector3 pos)
    { 
        homePoint = pos; 
    }

    private void FixedUpdate()
    {
        // dont do anything until data has been set
        if (data == null) return;

        switch (state)
        {
            case MobState.Roam:
                Roam();
                break;
            case MobState.Idle:
                Idle();
                break;
            case MobState.ReturningHome:
                ReturnHome();
                break;
            case MobState.Chase:
                ChasePlayer();
                break;
        }
    }

    private float GetDistFromPos(Vector3 pos)
    {
        return Vector2.Distance(transform.position, pos);
    }

    private void StartReturningHome()
    {
        state = MobState.ReturningHome;
        movement.SetMotionVector(homePoint);
    }

    private void ReturnHome()
    {
        movement.Move();

        // reached home point
        if (GetDistFromPos(homePoint) < 1) StartIdle();
    }

    private void StartIdle()
    {
        state = MobState.Idle;
        // get random number from 0 to max idle time
        idleTimer = Random.Range(0, maxIdleTime);
        movement.SetMotionless();
    }

    private void Idle()
    {
        idleTimer -= Time.deltaTime;

        // idle timer hits 0
        if (idleTimer <= 0) StartRoam();
    }

    private void StartRoam()
    {
        state = MobState.Roam;

        // get a random position around the home point
        Vector2 randPoint = (Vector2) homePoint + Random.insideUnitCircle * homeRadius;
        movement.SetMotionVector(randPoint);
    }

    private void Roam()
    {
        movement.Move();
        // reached target roam position
        if (GetDistFromPos(targetPos) < 1) StartIdle();
    }

    private void ChasePlayer()
    {
        if (plr == null) return;

        // distance from mob to player
        float dist = GetDistFromPos(plr.transform.position);

        // if player is out of the chase range
        if (dist > data.chaseRange)
        {
            // stop chasing player, and set motion vector towards home point
            plr = null;
            StartReturningHome();
        }

        // if player is within attack range
        else if (dist < data.attackRange)
        {
            // stop moving and perform attack
            Attack();
        } 
        
        // else move towards player
        else
        {
            movement.SetMotionVector(plr.transform.position);
            movement.Move();
        }
    }

    private void Attack()
    {
        bool success = combat.Attack(data.damage);

        // if attack was successful
        if (success)
        {
            state = MobState.Attack;
            // play attack animation
            // when attack animation is finished, set state back to "Chase"
        }

    }

    private void Die()
    {
        // drop mob food
        GameObject food = Instantiate(itemPrefab, transform);
        Item item = food.GetComponent<Item>();
        food.name = data.foodDrop.name;

        item.SetData(data.foodDrop);

        food.transform.SetParent(null, true);

        // play die animation if there is one
        // after some delay, destroy mob object
    }

    // player enters mob's collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (plr == null && collision.CompareTag("Player"))
        {
            state = MobState.Chase;
            plr = collision.gameObject;
        }
    }
}
