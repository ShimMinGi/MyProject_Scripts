using UnityEngine;

public abstract class StateMachine
{
    protected IState currentState;

    public void TransitState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        if (currentState == null)
        {
            return;
        }

        currentState.PhysicsUpdate();
    }
}