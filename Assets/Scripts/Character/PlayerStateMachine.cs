using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public bool isTalking;

    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private DialogLoader dialogue;
    [SerializeField] public List<CorruptedStateMachine> corrupteds { get; private set; } = new List<CorruptedStateMachine>(); 
    [SerializeField] public Health health { get; private set; }

    [SerializeField] Inventory inventory;
    [SerializeField] bool inventoryDisplayMode;
    [SerializeField] CollectiblesGenerator generator;

    private float x, y;
    private Vector3 localScale;
    private Vector2 direction;
    private bool isFacingRight = true;
    [SerializeField] private Vector2 speed;

    bool stillAttacking;
    bool continueAttack;
    private void Awake()
    {
        dialogue = GetComponent<DialogLoader>();

        inventory = gameObject.AddComponent<Inventory>();
        inventory.Initialize(GameObject.Find("Player Inventory"));
        inventory.DisplayInventory(inventoryDisplayMode);

        inventory.AddCollectible(ScriptableObject.CreateInstance<Item>().Create(ItemId.ArtefactFragment));

        this.tag = "Player";
        states.Add(new State(CharacterEnumState.Idle,IdleEnter, IdleInput, IdleUpdate, IdleExit));
        states.Add(new State(CharacterEnumState.Move, MoveEnter, MoveInput, MoveUpdate, MoveExit));
        states.Add(new State(CharacterEnumState.Dialogue, DialogEnter, () => { }, DialogUpdate, DialogExit));
        states.Add(new State(CharacterEnumState.Looting, LootingEnter, LootingInput, LootingUpdate, LootingExit));

    }

    private void Start()
    {
        currentState = states[0];
        currentState.EnterHandleState();
    }

    private void Update()
    {
        currentState.InputHandleState();
        currentState.UpdateHandleState();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (generator != null) generator.DisplayInventory();
        }
    }

    #region StateInputCondition
    private bool MoveState()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        if (x != 0 || y != 0)
        {
            ChangeState(CharacterEnumState.Move);
            return true;
        }
        return false;
    }

    private bool DialogueState()
    {
        if (dialogue != null && Input.GetKeyDown(KeyCode.E)) 
        {
            ChangeState(CharacterEnumState.Dialogue);
            return true; 
        }
        return false;
    }

    private bool LootingState()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (!inventoryDisplayMode)
            {
                ChangeState(CharacterEnumState.Looting);
                return true;
            }
            else
            {
                ChangeState(CharacterEnumState.Idle);
                return true;
            }
        }
        return false;
    }


    #endregion

    #region IdleState
    private void IdleEnter()
    {
        Debug.Log("Character is Entering Idle State");
    }
    private void IdleInput()
    {
        if (LootingState()) return;
        if (DialogueState()) return;
        if (MoveState()) return;
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
        if (LootingState()) return;
        if (DialogueState()) return;
        if (MoveState()) return;
        ChangeState(CharacterEnumState.Idle);
    }
    private void MoveUpdate()
    {
        x *=  speed.x;
        y *= speed.y;
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

    #region DialogState
    private void DialogEnter()
    {
        Debug.Log("Character is Entering Dialog State");
    }

    public void DialogInput(bool isDone)
    {
        if (isDone) ChangeState(CharacterEnumState.Idle);
    }

    private void DialogUpdate()
    {
        Debug.Log("Character is in Dialog State");
        if (isTalking)
        {
            Debug.Log("Done");
        }
    }

    private void DialogExit()
    {
        Debug.Log("Character is Exiting Dialog State");
    }
    #endregion

    #region LootingState
    private void LootingEnter()
    {
        inventoryDisplayMode = true;
        inventory.DisplayInventory(inventoryDisplayMode);
    }
    private void LootingInput()
    {
        if (LootingState()) return;
    }
    private void LootingUpdate()
    {
        if (Input.GetKeyDown(KeyCode.A)) inventory.ChangeSlots(-1);
        if (Input.GetKeyDown(KeyCode.D)) inventory.ChangeSlots(1);

        if (Input.GetKeyDown(KeyCode.W)) inventory.ChangeSeeds(1);
        if (Input.GetKeyDown(KeyCode.S)) inventory.ChangeSeeds(-1);

        if (Input.GetKeyDown(KeyCode.E)) inventory.UseCollectible();
    }
    private void LootingExit()
    {
        inventoryDisplayMode = false;
        inventory.DisplayInventory(inventoryDisplayMode);
    }
    #endregion


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Corrupted")
        {
            corrupteds.Add(collision.GetComponent<CorruptedStateMachine>());
        }

        if (collision.tag == "CollectibleGenerator")
        {
            generator = collision.GetComponent<CollectiblesGenerator>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Corrupted")
        {
            corrupteds.Remove(collision.GetComponent<CorruptedStateMachine>());
        }
        if (collision.tag == "CollectibleGenerator")
        {
            generator = null;
        }
    }
}
