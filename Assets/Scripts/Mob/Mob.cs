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
    Dead,
}

public class Mob : MonoBehaviour
{
    private Vector3 homePoint; // the position the mob will always go back to when not chasing
    public int homeRadius; // the radius around the home point which the mob will roam around
    private Vector2 targetPos; // target position while in roam state

    private float idleTimer;
    public float maxIdleTime; // max time for standing still

    public Color damageColor;

    // systems
    private MobMovement movement;
    private Health health;
    private Combat combat;

    private Animator animator;
    private SpriteRenderer spriteRender;
    private GameObject plr;

    public MobData data;
    private MobState state = MobState.Roam;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        combat = GetComponent<Combat>();
        movement = GetComponent<MobMovement>();
        spriteRender = GetComponent<SpriteRenderer>(); 
        homePoint = transform.position;

        if (data != null) SetData(data);
    }

    private void Start()
    {
        SetHomePoint(transform.position);
        StartRoam();
    }

    public void SetData(MobData data)
    {
        this.data = data;
        health.SetMaxHealth(data.maxHp);
        combat.Setup(data.hitboxRadius, data.attackCooldown);

        health.OnDeath += Die; // call Die() method when health is 0
        health.OnDamage += TakeDamage; // when mob takes damage

        movement.speed = data.speed;
        if (animator == null) spriteRender.sprite = data.sprite;
    }

    public void SetHomePoint(Vector3 pos)
    {
        homePoint = pos;
    }

    private void Update()
    {
        // dont do anything until data has been set
        if (data == null) return;

        switch (state)
        {
            case MobState.Dead:
                break;
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

    private void FixedUpdate()
    {
        movement.Move();
    }

    private float GetDistFromPos(Vector3 pos)
    {
        return Vector2.Distance(transform.position, pos);
    }

    private void StartReturningHome()
    {
        //Debug.Log("returning home");
        state = MobState.ReturningHome;
        movement.SetMotionVector(homePoint);
    }

    private void ReturnHome()
    {
        // reached home point
        if (GetDistFromPos(homePoint) < 5) StartIdle();
    }

    private void StartIdle()
    {
        //Debug.Log("Start idle");
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
        //Debug.Log("Start roam");
        state = MobState.Roam;

        // get a random position around the home point
        Vector2 randPoint = (Vector2) homePoint + Random.insideUnitCircle * homeRadius;
        movement.SetMotionVector(randPoint);
        targetPos = randPoint;
    }

    private void Roam()
    {
        // reached target roam position
        if (GetDistFromPos(targetPos) < 5) StartIdle();
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
            movement.SetMotionless();
            Attack();
        } 
        
        // else move towards player
        else
        {
            movement.SetMotionVector(plr.transform.position + Vector3.up * 3.5f);
        }
    }

    private void Attack()
    {
        bool success = combat.Attack(data.damage);

        // if attack was successful
        if (success)
        {
            state = MobState.Attack;
            if (animator != null) animator.SetTrigger("Attack");
            if (animator == null) Invoke("EndAttack", data.attackCooldown);
        }
        
    }

    public void AttackAnimationFinished()
    {
        Invoke("EndAttack", data.attackCooldown);
    }

    //temporary
    private void EndAttack()
    {
        if (state == MobState.Dead) return;
        state = MobState.Chase;
    }

    private void TakeDamage()
    {
        if (spriteRender != null) spriteRender.color = damageColor;
        if (animator != null) animator.SetTrigger("Hurt");
        Invoke("StopDamage", 0.2f);
    }

    private void StopDamage()
    {
        if (state == MobState.Dead) return;
        if (spriteRender != null)  spriteRender.color = new Color(1f, 1f, 1f);
    }

    private void Die()
    {
        // drop mob food
        ItemSpawner itemSpawner = GetComponent<ItemSpawner>();
        if (data.dropAmount > 1)
        {
            itemSpawner.SpawnRadius(data.foodDrops, null, transform.position, data.dropAmount, 5);
        } else
        {
            itemSpawner.SpawnRandom(data.foodDrops, null, transform.position);
        }

        state = MobState.Dead;
        movement.SetMotionless();

        if (spriteRender != null) spriteRender.enabled = false;
        if (animator == null) { DestroyMob(); } else
        {
            animator.SetBool("isDead", true);
            DeadAnimationFinished();
        }
    }

    public void DeadAnimationFinished()
    {
        Invoke("DestroyMob", 1);
    }

    private void DestroyMob()
    {
        gameObject.SetActive(false);
    }

    // player enters mob's collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (plr == null && collision.CompareTag("Player"))
        {
            //Debug.Log("Start chasing");
            state = MobState.Chase;
            plr = collision.gameObject;
        }
    }

    // draw helpful gizmos
    private void OnDrawGizmosSelected()
    {
        if (data == null) return;

        // draw home point
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(homePoint, homeRadius);

        // draw chase range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, data.chaseRange);

        Gizmos.color = Color.yellow;

        switch(state)
        {
            case MobState.Chase:
                Gizmos.DrawLine(transform.position, plr.transform.position);
                break;
            case MobState.ReturningHome:
                Gizmos.DrawLine(transform.position, homePoint);
                break;
            case MobState.Roam:
                Gizmos.DrawLine(transform.position, targetPos);
                break;
        }
    }
}
