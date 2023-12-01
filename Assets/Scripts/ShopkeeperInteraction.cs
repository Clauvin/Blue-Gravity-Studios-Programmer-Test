using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopkeeperInteraction : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        MainCharacterControl mainCharacterControl = GetMainCharacterFromCollider(collider);
        TryToStartConversationWithPlayerCharacter(mainCharacterControl);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        MainCharacterControl mainCharacterControl = GetMainCharacterFromCollider(collider);
        TryToStartConversationWithPlayerCharacter(mainCharacterControl);
    }

    private void TryToStartConversationWithPlayerCharacter(MainCharacterControl characterControl)
    {
        if (characterControl.isTryingToInteract)
        {
            characterControl.OutsiderSetsInteracting(this.gameObject, true);
            Debug.Log("try to start conversation");
        }
        
    }

    private MainCharacterControl GetMainCharacterFromCollider(Collider2D collider)
    {
        return collider.gameObject.GetComponentInParent<MainCharacterControl>();
    }
}
