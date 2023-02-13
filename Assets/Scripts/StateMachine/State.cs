using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class State
{
    protected Action[] actionMethod = new Action[4];
    public CharacterEnumState stateName { get; private set; }
    public delegate void Action();

    public State(CharacterEnumState newStateName, Action enterAction, Action inputAction, Action updateAction, Action exitAction)
    {
        stateName = newStateName;
        actionMethod[0] = enterAction;
        actionMethod[1] = inputAction;
        actionMethod[2] = updateAction;
        actionMethod[3] = exitAction;
    }

    protected void onStateChange(State oldState, State newState)
    {
        if (oldState == newState) return;
        oldState.ExitHandleState();
        newState.EnterHandleState();
    }

    public void EnterHandleState()
    {
        actionMethod[0]?.Invoke();
    }

    public void InputHandleState()
    {
        actionMethod[1]?.Invoke();
    }

    public void UpdateHandleState()
    {
        actionMethod[2]?.Invoke();
    }

    public void ExitHandleState()
    {
        actionMethod[3]?.Invoke();
    }
}

public enum CharacterEnumState
{
    Idle, Move, Dialogue, Attacking, Damaged, Looting
}
