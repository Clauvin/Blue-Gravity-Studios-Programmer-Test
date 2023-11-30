using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacterMovement : MonoBehaviour
{
    public InputActionAsset actions;

    private InputAction moveAction;

    private Animator animator;

    [Range(0, 10)]
    public float constantSpeedPerSecond = 3.0f;

    void Awake()
    {
        moveAction = actions.FindActionMap("Main Character Movement").FindAction("movement");

        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        MoveCharacter(moveVector);
        UpdateAnimation(moveVector);


    }

    private void UpdateAnimation(Vector2 moveVector)
    {
        if (moveVector.x != 0 || moveVector.y != 0)
        {
            animator.SetFloat("X", moveVector.x);
            animator.SetFloat("Y", moveVector.y);
        }
    }

    private void MoveCharacter(Vector2 moveVector)
    {
        

        //debt: should I clarify the code here further or is this enough?
        Vector3 newMoveVector = (Vector3)moveVector * constantSpeedPerSecond * Time.deltaTime;

        Vector3 oldCharacterPosition = GetComponent<Transform>().position;

        Vector3 newCharacterPosition = oldCharacterPosition + newMoveVector;

        GetComponent<Transform>().position = newCharacterPosition;
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
