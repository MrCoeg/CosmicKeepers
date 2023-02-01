using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public State currentState { get; protected set; }

    protected void ChangeState(State newState)
    {
        currentState = newState;
    }
}
