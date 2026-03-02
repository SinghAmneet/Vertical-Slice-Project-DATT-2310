
using UnityEngine;

public class StateMachine
{
    public State currentState;

    public void Setup(State startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(State newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}
