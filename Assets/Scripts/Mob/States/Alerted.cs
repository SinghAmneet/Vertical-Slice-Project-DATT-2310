using UnityEngine;

public class Alerted : State
{
    private float endTimer;
    private float timeToEnd = 1;
    private GameObject alertUI;

    public Alerted(StateMachine stateMachine, Mob mob, GameObject alertUI) : base(stateMachine, mob) { 
        this.alertUI = alertUI;
        alertUI.SetActive(false);
    }

    public override void Enter()
    {
        mob.movement.SetMotionless();
        alertUI.SetActive(true);
        endTimer = timeToEnd;
    }

    public override void Exit()
    {
        alertUI.SetActive(false);
        //Debug.Log("stopped alerted ");
    }

    public override void Update()
    {
        endTimer -= Time.deltaTime;
        if (endTimer < 0) stateMachine.ChangeState(mob.chase);
    }
}
