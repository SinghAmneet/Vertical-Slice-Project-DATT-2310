
public class Chase : State
{
    public Chase(StateMachine stateMachine, Mob mob) : base(stateMachine, mob) { }

    public override void Update()
    {
        float dist = mob.GetDistFromPlr();
        
        if (!mob.PlrInChaseRange())
        {
            // start roam or going home
            //mob.SetTargetPos(mob.homePoint);
            stateMachine.ChangeState(mob.goingHome);
        }
        else if (mob.PlrInAttackRange())
        {
            stateMachine.ChangeState(mob.attack);
        }

        else

        {
            //if (dist < 15 * mob.transform.localScale.x)
            //{
            //    if (mob.IsAbovePlr())
            //    {
            //        mob.GoOppositeOfPlr();
            //    }
            //    else if (mob.IsBelowPlr())
            //    {
            //        mob.GoOppositeOfPlr();
            //    }
            //    else
            //    {
            //        mob.GoTowardsPlr();
            //    }
            //} else
            //{
            //    mob.GoTowardsPlr();
            //}
            mob.GoTowardsPlr();
        }
    }
}
