using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StateMachine : MonoBehaviour
{
    [SerializeField] protected State currentState;
    [SerializeField] protected List<State> states = new List<State>();

    protected void ChangeState(CharacterEnumState newState)
    {
        currentState.ExitHandleState();
        currentState = states.Find(x => x.stateName == newState);
        currentState.EnterHandleState();
    }
}
