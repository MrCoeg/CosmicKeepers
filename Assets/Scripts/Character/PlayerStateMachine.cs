using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    List<State> states = new List<State>();

    public bool isDialog;

    private void Awake()
    {
        states.Add(new IdleState(IdleEnter, IdleUpdate, IdleExit, states));
        states.Add(new MoveState(MoveEnter, MoveUpdate, MoveExit, states));
        states.Add(new DialogState(DialogEnter, DialogUpdate, DialogExit, states));
    }

    private void Start()
    {
        currentState = states[0];
        currentState.EnterHandleState();
    }

    private void Update()
    {
        currentState = currentState.InputHandleState(states[0]);
        currentState.UpdateHandleState();
    }

    #region MoveState
    private void MoveEnter()
    {
        Debug.Log("Character is Entering Move State");
    }

    private float x, y;
    private Vector3 localScale;
    private Vector2 direction;
    private bool isFacingRight = true;
    [SerializeField] private Vector2 speed;
    private void MoveUpdate()
    {
        x = Input.GetAxis("Horizontal") * speed.x;
        y = Input.GetAxis("Vertical") * speed.y;
        direction = new Vector2(x, y);
        localScale = transform.localScale;

        if(x < 0 && isFacingRight)
        {
            isFacingRight = false;
            localScale.x *= -1;
        }else if(x > 0 && !isFacingRight)
        {
            isFacingRight = true;
            localScale.x *= -1;
        }

        transform.localScale = localScale;
        transform.Translate(direction * Time.deltaTime);
    }
    private void MoveExit()
    {
        Debug.Log("Character is Exiting Move State");
    }
    #endregion

    #region IdleState
    private void IdleEnter()
    {
        Debug.Log("Character is Entering Idle State");
    }

    private void IdleUpdate()
    {
        Debug.Log("Character is in Idle State");
    }
    private void IdleExit()
    {
        Debug.Log("Character is Exiting Idle State");
    }
    #endregion

    #region DialogState
    private void DialogEnter()
    {
        Debug.Log("Character is Entering Dialog State");
    }

    private void DialogUpdate()
    {
        Debug.Log("Character is in Dialog State");
        if (isDialog)
        {
            Debug.Log("Done");
        }
    }

    private void DialogExit()
    {
        Debug.Log("Character is Exiting Dialog State");
    }
    #endregion
}

public enum PlayerEnumState{
    Idle,Move
}
