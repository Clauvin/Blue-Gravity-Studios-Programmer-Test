using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;
using Yarn.Unity;

public class MainCharacterControl : MonoBehaviour
{
    public InputActionAsset actions;

    private InputAction moveAction;
    private InputAction accessInventoryAction;
    private InputAction interactAction;

    private Animator animator;
    public GameObject equippedSpritesRoot;

    private Rigidbody2D rigidbody2D;

    private GameObject eventCollider2DObject;

    private GameObject inventoryPanel;

    [Range(0, 10)]
    private float constantSpeedPerSecond = 3.0f;

    public bool isInteracting { get; private set; }
    public bool isTryingToInteract { get; private set; }

    #region MainCharacterConsts
    private const string nameOfActionMap = "Main Character Actions";
    private const string nameOfMovementAction = "movement";
    private const string nameOfAccessInventoryAction = "accessInventory";
    private const string nameOfInteractAction = "interact";
    private readonly string[] namesOfEquippableCategories = { "Clothes", "Hat", "Wig" };
    #endregion

    public void OutsiderSetsInteracting(GameObject who, bool value)
    {
        Debug.Log(who + " sets main character isInteracting as " + value);
        isInteracting = value;
        if (isInteracting) { DisableMovementActions(); }
        else { EnableMovementActions(); }
    }

    void Awake()
    {
        InputActionMap mainCharacterActionsMap = actions.FindActionMap(nameOfActionMap);
        moveAction = mainCharacterActionsMap.FindAction(nameOfMovementAction);
        accessInventoryAction = mainCharacterActionsMap.FindAction(nameOfAccessInventoryAction);
        interactAction = mainCharacterActionsMap.FindAction(nameOfInteractAction);

        animator = GetComponent<Animator>();

        rigidbody2D = GetComponent<Rigidbody2D>();

        BoxCollider2D[] boxCollidersFoundOnTheMainCharacter = GetComponentsInChildren<BoxCollider2D>();

        //debt: there are only two GameObjects BY NOW on the main Character. We need to be more specific in terms of how to find the
        //  EventCollider BoxCollider2D.
        eventCollider2DObject = boxCollidersFoundOnTheMainCharacter[boxCollidersFoundOnTheMainCharacter.Length - 1].gameObject;

        inventoryPanel = GameObject.Find("Inventory System");
    }

    void Start()
    {

    }

    void FixedUpdate()
    {
        Vector2 moveVector = moveAction.ReadValue<Vector2>();
        float tryingToAccessInventory = accessInventoryAction.ReadValue<float>();
        float tryingToInteract = interactAction.ReadValue<float>();

        if (tryingToInteract > 0.0f) { isTryingToInteract = true; } else { isTryingToInteract = false; }
        
        if (!isInteracting)
        {
            MoveCharacter(moveVector);
            MoveEventColliderBox(moveVector);
            AccessInventory(tryingToAccessInventory);
        }

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
            float defaultOffsetValue = 0.18f;

            if (moveVector.x < 0) { xOffset = -defaultOffsetValue; }
            else if (moveVector.x > 0) { xOffset = defaultOffsetValue; }
            
            if (moveVector.y < 0) { yOffset = -defaultOffsetValue; }
            else if (moveVector.y > 0) { yOffset = defaultOffsetValue; }

            eventCollider2DObject.GetComponent<BoxCollider2D>().offset = new Vector2(xOffset, yOffset);
        }
    }

    private void AccessInventory(float tryingToAccessInventory)
    {
        if (tryingToAccessInventory > 0.0f) {
            inventoryPanel.GetComponent<InventoryUI>().OpenInventoryInterface();
            OutsiderSetsInteracting(this.gameObject, true);
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

        if (GetComponent<Inventory>().GetAmountOfEquippedItems() != 0)
        {
            string currentBodySpriteName = GetComponent<SpriteRenderer>().sprite.name;
            string spriteSuffix = currentBodySpriteName.Replace("char1_walk", "");
            List<ShopItem> equippedItems = GetComponent<Inventory>().GetEquippedItems();

            foreach (string category in namesOfEquippableCategories)
            {
                bool foundCategory = false;
                foreach (ShopItem item in equippedItems)
                {
                    if (category == item.category.Name)
                    {
                        SpriteResolver equipmentSpriteResolver = equippedSpritesRoot.transform.Find(category).gameObject.GetComponent<SpriteResolver>();

                        string equipmentSpritePrefix = item.name;
                        string equipmentSpriteName = equipmentSpritePrefix + spriteSuffix;
                        equipmentSpriteResolver.SetCategoryAndLabel(equipmentSpritePrefix, equipmentSpriteName);
                        foundCategory = true;
                        break;
                    }
                }
                if (foundCategory == false)
                {
                    SpriteResolver equipmentSpriteResolver = equippedSpritesRoot.transform.Find(category).gameObject.GetComponent<SpriteResolver>();
                    equipmentSpriteResolver.SetCategoryAndLabel("No Clothes", "none");
                }
            }
        }
        else
        {
            foreach (string category in namesOfEquippableCategories)
            {
                SpriteResolver equipmentSpriteResolver = equippedSpritesRoot.transform.Find(category).gameObject.GetComponent<SpriteResolver>();
                equipmentSpriteResolver.SetCategoryAndLabel("No Clothes", "none");
            }
        }
    }

    private void EnableMovementActions()
    {
        actions.FindActionMap(nameOfActionMap).FindAction(nameOfMovementAction).Enable();
    }

    private void DisableMovementActions()
    {
        actions.FindActionMap(nameOfActionMap).FindAction(nameOfMovementAction).Disable();
    }

    //debt: had to expose the function so we could call it through Yarn. This MAY be an issue in the future.
    [YarnCommand("TryToEndConversationWithPlayerCharacter")]
    public void TryToEndConversationWithPlayerCharacter()
    {
        if (isInteracting)
        {
            OutsiderSetsInteracting(this.gameObject, false);
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
