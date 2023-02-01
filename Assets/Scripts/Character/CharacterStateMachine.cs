using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : StateMachine
{
    private void Start()
    {
        currentState = new IdleState();
        currentState.EnterHandleState();
    }

    private void Update()
    {
        currentState.InputHandleState();
        currentState.UpdateHandleState();
    }
}
