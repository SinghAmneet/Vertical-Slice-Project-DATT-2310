
public class Dead : State
{
    public Dead(StateMachine stateMachine, Mob mob) : base(stateMachine, mob) { }

    public override void Enter()
    {
        mob.movement.EndDash();
        mob.movement.SetMotionless();
        mob.PlayDieAnimation();
    }

}
