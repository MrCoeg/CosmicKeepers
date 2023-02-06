using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected Action[] actionMethod = new Action[3];
    protected List<State> stateMachineStates;
    public delegate void Action();

    protected void onStateChange(State oldState, State newState)
    {
        if (oldState == newState) return;
        oldState.ExitHandleState();
        newState.EnterHandleState();
    }

    public virtual void EnterHandleState()
    {
        actionMethod[0]?.Invoke();
    }

    public virtual State InputHandleState(State previousState)
    {
        return this;
    }

    public virtual void UpdateHandleState()
    {
        actionMethod[1]?.Invoke();
    }

    public virtual void ExitHandleState()
    {
        actionMethod[2]?.Invoke();
    }
}
