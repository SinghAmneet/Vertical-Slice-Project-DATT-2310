
public class Idle : State
{
    public Idle(StateMachine stateMachine, Mob mob) : base(stateMachine, mob) { }

    public override void Enter()
    {
        mob.SetRandIdle();
        mob.movement.SetMotionless();
    }

    public override void Update()
    {
        mob.DecreaseIdleTimer();
        if (mob.IdleFinished())
        {
            stateMachine.ChangeState(mob.roam);
        }
    }
}
