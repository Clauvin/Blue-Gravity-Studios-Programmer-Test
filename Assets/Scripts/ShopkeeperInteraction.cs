using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ShopkeeperInteraction : MonoBehaviour
{
    private const string startingConversationNode = "Start";

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
        if (characterControl.isTryingToInteract && !characterControl.isInteracting)
        {
            characterControl.OutsiderSetsInteracting(this.gameObject, true);
            GetComponentInChildren<DialogueRunner>().StartDialogue(startingConversationNode);
        }
        
    }

    //debt: had to expose the function so we could call it through Yarn. This MAY be an issue in the future.
    [YarnCommand("TryToEndConversationWithPlayerCharacter")]
    public void TryToEndConversationWithPlayerCharacter(MainCharacterControl characterControl)
    {
        if (characterControl.isInteracting)
        {
            characterControl.OutsiderSetsInteracting(this.gameObject, false);

        }
    }

    private MainCharacterControl GetMainCharacterFromCollider(Collider2D collider)
    {
        return collider.gameObject.GetComponentInParent<MainCharacterControl>();
    }
}
