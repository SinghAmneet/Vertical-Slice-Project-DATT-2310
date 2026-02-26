
public abstract class State
{
    protected StateMachine stateMachine;
    protected Mob mob;
    public State(StateMachine stateMachine, Mob mob)
    {
        this.stateMachine = stateMachine;
        this.mob = mob;
    }

    // when entering state
    public virtual void Enter() { }

    // when leaving state
    public virtual void Exit() { }

    // when updating state every frame
    public virtual void Update() { }
}
