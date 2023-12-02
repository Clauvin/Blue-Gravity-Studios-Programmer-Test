using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public interface IInteracter
{
    public void TryToStartConversationWithPlayerCharacter(MainCharacterControl characterControl, string conversationNode);
}

public class ShopkeeperInteraction : MonoBehaviour, IInteracter
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
        TryToStartConversationWithPlayerCharacter(mainCharacterControl, startingConversationNode);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        MainCharacterControl mainCharacterControl = GetMainCharacterFromCollider(collider);
        TryToStartConversationWithPlayerCharacter(mainCharacterControl, startingConversationNode);
    }

    public void TryToStartConversationWithPlayerCharacter(MainCharacterControl characterControl, string conversationNode)
    {
        if (characterControl.isTryingToInteract && !characterControl.isInteracting)
        {
            characterControl.OutsiderSetsInteracting(this.gameObject, true);
            GetComponentInChildren<DialogueRunner>().StartDialogue(conversationNode);
        }
    }

    private MainCharacterControl GetMainCharacterFromCollider(Collider2D collider)
    {
        return collider.gameObject.GetComponentInParent<MainCharacterControl>();
    }
}
