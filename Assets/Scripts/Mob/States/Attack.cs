using UnityEngine;
public class Attack : State
{
    private bool playingAnim;
    private float startTimer;
    private float? cooldownTimer;
    public Attack(StateMachine stateMachine, Mob mob) : base(stateMachine, mob) { }

    public override void Enter()
    {
        cooldownTimer = null;
        startTimer = mob.data.attackCooldown;
        mob.movement.SetMotionless();
    }

    private void StartAttack()
    {
        playingAnim = true;
        mob.PlayAttackAnimation();
    }

    public void StartCooldown()
    {
        playingAnim = false;
        cooldownTimer = mob.data.attackCooldown;
    }

    public void EndAttack() 
    {
        cooldownTimer = null;
        if (mob.PlrInAttackRange())
        {
            StartAttack();
        } else
        {
            stateMachine.ChangeState(mob.chase);
        }
    }

    public override void Exit()
    {
        playingAnim = false;
        cooldownTimer = null;
    }

    public override void Update()
    {
        if (playingAnim) return;
        // not on cooldown
        if (cooldownTimer == null)
        {
            startTimer -= Time.deltaTime;
            if (startTimer < 0)
            {
                //EndAttack();
                StartAttack();
            }
        } else
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0) EndAttack();
        }
        
    }
}
