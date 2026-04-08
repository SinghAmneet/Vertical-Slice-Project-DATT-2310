using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public Vector3 homePoint { get; private set; } // the position the mob will always go back to when not chasing
    public int homeRadius; // the radius around the home point which the mob will roam around
    private Vector2 targetPos; // target position while in roam state

    public Transform rootPoint;
    private Vector3 offset;

    public float itemDropRadius = 5;

    private float idleTimer;
    public float maxIdleTime; // max time for standing still

    public GameObject alertUI;
    public AudioSource attackSound;

    // systems
    public MobMovement movement { get; private set; }
    public Health health { get; private set; }
    public Combat combat { get; private set; }

    private Animator animator;
    public GameObject plr;

    // mob data
    public MobData data;

    // states
    private StateMachine stateMachine = new();
    public Idle idle;
    public Roam roam;
    public Chase chase;
    public GoingHome goingHome;
    public Dead dead;
    public Attack attack;
    public Alerted alerted;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        combat = GetComponent<Combat>();
        movement = GetComponent<MobMovement>();

        offset = rootPoint.position - transform.position;

        if (data != null) SetData(data);
        SetStates();
    }

    private void Start()
    {
        SetHomePoint(transform.position);
        stateMachine.ChangeState(roam);
    }

    private void Update()
    {
        // dont do anything until data has been set
        if (data == null) return;
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        if (plr == null)
        {
            movement.SetDirection();
        } else
        {
            movement.SetFacingDirection(plr.transform.position);
        }
            movement.Move();
    }

    // player enters mob's collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (stateMachine.currentState is Chase || stateMachine.currentState is Attack || stateMachine.currentState is Alerted) return;
            plr = collision.gameObject;
            combat.SetTarget(plr);
            stateMachine.ChangeState(alerted);
        }
    }

    /*
     * Setter methods
     */
    public void SetData(MobData data)
    {
        this.data = data;
        health.SetMaxHealth(data.maxHp);

        health.OnDeath += Die; // invokes when health is 0
        health.OnDamage += TakeDamage; // invokes when mob takes damage

        movement.speed = data.speed;
        movement.SetDirectionOffset(data.forwardDirection);
    }

    private void SetStates()
    {
        Mob mob = GetComponent<Mob>();
        idle = new Idle(stateMachine, mob);
        roam = new Roam(stateMachine, mob);
        chase = new Chase(stateMachine, mob);
        goingHome = new GoingHome(stateMachine, mob);
        dead = new Dead(stateMachine, mob);
        attack = new Attack(stateMachine, mob);
        alerted = new Alerted(stateMachine, mob, alertUI);
    }

    public void SetHomePoint(Vector3 pos)
    {
        homePoint = pos;
    }

    public void SetRandIdle()
    {
        idleTimer = Random.Range(0, maxIdleTime);
    }

    public void SetTargetPos(Vector3 pos)
    {
        targetPos = pos;
    }

    /*
     * Distance methods
     */
    public float GetDistFromPos(Vector3 pos)
    {  return Vector2.Distance(rootPoint.transform.position, pos); }

    public float GetDistFromTarget() 
    { return GetDistFromPos(targetPos); }

    public float GetDistFromHome() 
    { return GetDistFromPos(homePoint); }

    public float GetDistFromPlr() 
    {
        //float modifier = 3.5f;
        //if (IsBelowPlr()) { modifier = 3.5f; } else if (IsAbovePlr()) { modifier = 3.5f; }
        return GetDistFromPos(plr.transform.position + Vector3.up * 3.5f); 
    }

    public bool PlrInChaseRange() 
    { return GetDistFromPlr() < data.chaseRange; }

    public bool PlrInAttackRange()
    { return GetDistFromPlr() < data.attackRange; }

    public void GoTowardsPlr()
    {
        movement.SetMotionVector(plr.transform.position + Vector3.up * 3.5f);
    }

    public void GoOppositeOfPlr()
    {
        movement.SetMotionVector(-plr.transform.position + Vector3.up * 3.5f);
    }

    public Vector3 GetPlrDirection()
    {
        return (plr.transform.position + Vector3.up * 3.5f - rootPoint.transform.position).normalized;
    }

    private float GetYDiff()
    {
        return transform.position.y - plr.transform.position.y;
    }

    private float GetXDist()
    {
        return Mathf.Abs(transform.position.x - plr.transform.position.x);
    }

    public bool IsAbovePlr()
    {
        float yDiff = GetYDiff();
        return yDiff > 0 && Mathf.Abs(yDiff) > 0;
    }

    public bool IsBelowPlr()
    {
        float yDiff = GetYDiff();
        return yDiff < 0 && Mathf.Abs(yDiff) > 0;
    }

    public void DecreaseIdleTimer()
    {
        idleTimer -= Time.deltaTime;
    }

    public void SetPlayerNull()
    {
        plr = null;
    }

    public bool IdleFinished()
    {
        return idleTimer < 0;
    }

    public Vector2 GetRandHomePoint()
    {
        return (Vector2) homePoint + Random.insideUnitCircle * homeRadius;
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    // after attack animation stops, wait the attack cooldown before going back into chase state
    public void AttackAnimationFinished()
    { attack.StartCooldown(); }

    // event in Attack animation
    public void RegisterAttack()
    {
        combat.RegisterHits(data.damage);
    }

    public void PlayAttackSound()
    {
        attackSound.PlayOneShot(attackSound.clip);
    }

    public void StartDash()
    {
        attack.StartDash();
    }

    // subscribed to OnDamage event
    private void TakeDamage()
    {
        if (attack.IsAttacking()) return;
        animator.SetTrigger("Hurt");
    }

    // subscribed to OnDeath event
    private void Die() 
    { stateMachine.ChangeState(dead); }

    public void PlayDieAnimation()
    { animator.SetTrigger("Die"); }

    // event at the end of dead animation
    public void DeadAnimationFinished()
    {
        DropFood();
        DestroyMob();
    }

    private void DestroyMob() 
    { Destroy(gameObject); }
    //{ gameObject.SetActive(false); }

    public void DropFood()
    {
        ItemSpawner itemSpawner = GetComponent<ItemSpawner>();
        if (data.dropAmount > 1)
        {
            itemSpawner.SpawnRandom(data.foodDrops, null, rootPoint.position, data.dropAmount, itemDropRadius);
        }
        else
        {
            itemSpawner.SpawnRandom(data.foodDrops, null, rootPoint.position);
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
        Gizmos.DrawWireSphere(rootPoint.position, data.chaseRange);

        // draw attack range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(rootPoint.position, data.attackRange);

        Gizmos.color = Color.yellow;
        if (stateMachine.currentState is Chase)
        {
            Gizmos.DrawLine(rootPoint.transform.position, plr.transform.position);
        } else if (stateMachine.currentState is GoingHome)
        {
            Gizmos.DrawLine(rootPoint.transform.position, homePoint);
        } else if (stateMachine.currentState is Roam)
        {
            Gizmos.DrawLine(rootPoint.transform.position, targetPos);
        } else if (stateMachine.currentState is Attack)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(combat.GetHitboxPos(), combat.hitboxRadius);
        }

    }
}
