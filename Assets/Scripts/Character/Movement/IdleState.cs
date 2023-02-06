using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{

    public IdleState(Action enterAction, Action updateAction, Action exitAction, List<State> states)
    { 
        actionMethod[0] = enterAction;
        actionMethod[1] = updateAction;
        actionMethod[2] = exitAction;
        stateMachineStates = states;
    }

    public override void EnterHandleState()
    {
        base.EnterHandleState();
    }

    public override State InputHandleState(State previousState)
    {
        State next = stateMachineStates[(int)PlayerEnumState.Move].InputHandleState(this);
        onStateChange(this, next);
        return next;
    }

    public override void UpdateHandleState()
    {
        base.UpdateHandleState();
    }

    public override void ExitHandleState()
    {
        base.ExitHandleState();
    }
}
