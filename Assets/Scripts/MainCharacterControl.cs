using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacterControl : MonoBehaviour
{
    public InputActionAsset actions;

    private InputAction moveAction;
    private InputAction interactAction;

    private Animator animator;

    private Rigidbody2D rigidbody2D;

    private GameObject eventCollider2DObject;

    [Range(0, 10)]
    public float constantSpeedPerSecond = 3.0f;

    public bool isInteracting = false;

    #region MainCharacterConsts
    private const string nameOfActionMap = "Main Character Actions";
    private const string nameOfMovementAction = "movement";
    private const string nameOfInteractAction = "interact";
    #endregion

    void Awake()
    {
        InputActionMap mainCharacterActionsMap = actions.FindActionMap(nameOfActionMap);
        moveAction = mainCharacterActionsMap.FindAction(nameOfMovementAction);
        interactAction = mainCharacterActionsMap.FindAction(nameOfInteractAction);

        animator = GetComponent<Animator>();

        rigidbody2D = GetComponent<Rigidbody2D>();

        BoxCollider2D[] boxCollidersFoundOnTheMainCharacter = GetComponentsInChildren<BoxCollider2D>();

        //debt: there are only two GameObjects BY NOW on the main Character. We need to be more specific in terms of how to find the
        //  EventCollider BoxCollider2D.
        eventCollider2DObject = boxCollidersFoundOnTheMainCharacter[boxCollidersFoundOnTheMainCharacter.Length - 1].gameObject;
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        float tryingToInteract = interactAction.ReadValue<float>();

        if (!isInteracting)
        {
            MoveCharacter(moveVector);
            MoveEventColliderBox(moveVector);
            TryToInteract(tryingToInteract);
            UpdateAnimation(moveVector);
        }

    }

    private void MoveCharacter(Vector2 moveVector)
    {
        //debt: should I clarify the code here further or is this enough?
        Vector2 newMoveVector = moveVector * constantSpeedPerSecond * Time.fixedDeltaTime;

        Vector2 oldRigidBodyPosition = rigidbody2D.position;

        rigidbody2D.MovePosition(rigidbody2D.position + newMoveVector);
    }

    private void MoveEventColliderBox(Vector2 moveVector)
    {
        
        if (moveVector.x != 0 || moveVector.y != 0)
        {
            float xOffset = 0f, yOffset = 0f;
            float defaultOffsetValue = 0.18f;

            if (moveVector.x < 0) { xOffset = -defaultOffsetValue; }
            else if (moveVector.x > 0) { xOffset = defaultOffsetValue; }
            
            if (moveVector.y < 0) { yOffset = -defaultOffsetValue; }
            else if (moveVector.y > 0) { yOffset = defaultOffsetValue; }

            eventCollider2DObject.GetComponent<BoxCollider2D>().offset = new Vector2(xOffset, yOffset);
        }
    }

    private void TryToInteract(float tryingToInteract)
    {
        if (tryingToInteract > 0.0f)
        {
            Debug.Log(tryingToInteract);
        }
    }

    private void UpdateAnimation(Vector2 moveVector)
    {
        if (moveVector.x != 0 || moveVector.y != 0)
        {
            animator.SetFloat("X", moveVector.x);
            animator.SetFloat("Y", moveVector.y);
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }


    void OnEnable()
    {
        actions.FindActionMap(nameOfActionMap).Enable();
    }
    void OnDisable()
    {
        actions.FindActionMap(nameOfActionMap).Disable();
    }
}
