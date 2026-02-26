using UnityEngine;

public class Roam : State
{
    public Roam(StateMachine stateMachine, Mob mob) : base(stateMachine, mob) { }

    public override void Enter()
    {
        Vector2 randPoint = mob.GetRandHomePoint();
        mob.movement.SetMotionVector(randPoint);
        mob.SetTargetPos(randPoint);
    }

    public override void Update()
    {
        if (mob.GetDistFromTarget() < 3)
        {
            stateMachine.ChangeState(mob.idle);
        }
    }
}
