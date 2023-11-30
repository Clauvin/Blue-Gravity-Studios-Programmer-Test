using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacterControl : MonoBehaviour
{
    public InputActionAsset actions;

    private InputAction moveAction;

    private Animator animator;

    private Rigidbody2D rigidbody2D;

    private BoxCollider2D eventCollider2D;

    [Range(0, 10)]
    public float constantSpeedPerSecond = 3.0f;

    void Awake()
    {
        moveAction = actions.FindActionMap("Main Character Movement").FindAction("movement");

        animator = GetComponent<Animator>();

        rigidbody2D = GetComponent<Rigidbody2D>();

        eventCollider2D = GetComponentInChildren<BoxCollider2D>();
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        MoveCharacter(moveVector);
        MoveEventColliderBox(moveVector);
        UpdateAnimation(moveVector);
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

            if (moveVector.x < 0) { xOffset = -0.24f; }
            else if (moveVector.x > 0) { xOffset = 0.24f; }
            
            if (moveVector.y < 0) { yOffset = -0.24f; }
            else if (moveVector.y > 0) { yOffset = 0.24f; }

            Debug.Log(xOffset + " " + yOffset);

            eventCollider2D.offset.Set(xOffset, yOffset);
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
        actions.FindActionMap("Main Character Movement").Enable();
    }
    void OnDisable()
    {
        actions.FindActionMap("Main Character Movement").Disable();
    }
}
