using System;
using UnityEngine;
public class Attack : State
{
    private bool playingAnim;
    private float startTimer;
    private float? cooldownTimer;
    private float? dashTimer;

    private Vector3 dir;

    public Attack(StateMachine stateMachine, Mob mob) : base(stateMachine, mob) {  }

    public override void Enter()
    {
        cooldownTimer = null;
        startTimer = mob.data.attackCooldown;
        mob.movement.SetMotionless();
    }

    private void StartAttack()
    {
        //Debug.Log("Start");
        playingAnim = true;
        mob.PlayAttackAnimation();

        mob.PlayAttackSound();

        if (mob.data.hasDashAttack)
        {
            //mob.movement.SetDash(mob.GetPlrDirection() * mob.data.dashSpeed);
            dir = mob.GetPlrDirection() * mob.data.dashSpeed;
        }
    }

    public void StartDash()
    {
        if (!mob.data.hasDashAttack) return;
        mob.movement.SetDash(dir);
        dashTimer = mob.data.dashDuration;
    }

    public void StartCooldown()
    {
        //Debug.Log("cd");
        playingAnim = false;
        cooldownTimer = mob.data.attackCooldown;
        mob.combat.AttackEnd();
    }

    public void EndAttack() 
    {
        //Debug.Log("End");
        cooldownTimer = null;
        if (mob.PlrInAttackRange())
        {
            StartAttack();
        } else
        {
            stateMachine.ChangeState(mob.chase);
        }
    }

    public bool IsAttacking()
    {
        return playingAnim;
    }

    public override void Exit()
    {
        playingAnim = false;
        cooldownTimer = null;
        dashTimer = null;
    }

    public override void Update()
    {
        if (playingAnim)
        {
            if (dashTimer != null)
            {
                dashTimer -= Time.deltaTime;
                if (dashTimer < 0)
                {
                    mob.movement.EndDash();
                }
            }
            
            return;
        }

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
