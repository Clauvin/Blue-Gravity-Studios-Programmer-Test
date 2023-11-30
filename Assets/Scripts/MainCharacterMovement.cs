using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCharacterMovement : MonoBehaviour
{
    public InputActionAsset actions;

    private InputAction moveAction;

    void Awake()
    {
        moveAction = actions.FindActionMap("Main Character Movement").FindAction("movement");
    }

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        Debug.Log(moveVector.x + " " + moveVector.y);
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
