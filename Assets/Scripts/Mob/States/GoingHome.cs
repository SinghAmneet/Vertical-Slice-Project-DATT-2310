using UnityEngine;

public class GoingHome : State
{
    public GoingHome(StateMachine stateMachine, Mob mob) : base(stateMachine, mob) { }

    public override void Enter()
    {
        mob.movement.SetMotionVector(mob.homePoint);
        //mob.SetTargetPos(randPoint);
    }

    public override void Update()
    {
        if (mob.GetDistFromHome() < 3) stateMachine.ChangeState(mob.idle);
    }
}
