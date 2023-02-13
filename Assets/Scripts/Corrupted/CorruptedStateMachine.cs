using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedStateMachine : StateMachine
{
    Health health;
    bool isDie;
    private void Awake()
    {
        states.Add(new State(CharacterEnumState.Idle, IdleEnter, IdleInput, IdleUpdate, IdleExit));
        states.Add(new State(CharacterEnumState.Move ,MoveEnter, MoveInput, MoveUpdate, MoveExit));
        states.Add(new State(CharacterEnumState.Damaged, DamagedEnter, DamagedInput, () => { }, DamagedExit));

    }


    #region IdleState
    private void IdleEnter()
    {
        Debug.Log("Character is Entering Idle State");
    }
    private void IdleInput()
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

    #region MoveState
    private void MoveEnter()
    {
        Debug.Log("Character is Entering Move State");
    }

    private void MoveInput()
    {

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

        if (x < 0 && isFacingRight)
        {
            isFacingRight = false;
            localScale.x *= -1;
        }
        else if (x > 0 && !isFacingRight)
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

    #region DamagedState

    public void TakeDamage(float damageAmount)
    {

        isDie = health.Damaged(damageAmount);
    }

    private void DamagedEnter()
    {

    }

    private void DamagedInput()
    {
    }

    private void DamagedExit()
    {
        if (!isDie) isDie = !isDie;
    }
    #endregion
}
