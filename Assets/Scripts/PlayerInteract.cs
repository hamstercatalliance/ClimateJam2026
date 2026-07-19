using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerInteract : MonoBehaviour
{
    public EventHandler InteractableActivate;
    [SerializeField] private GameInput gameInput;
    public class InteractableActivateEventArgs : EventArgs
    {
        public string npcID;

    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        string closestNPC = this.closestInteractableID();
        //Debug.Log("PlayerInteract: Interact action triggered, NPC = " + closestNPC);
        if (!DialogueBox.dialogueActive && closestNPC != null)
        {
          //Debug.Log("PlayerInteract: Interact action triggered, closest NPC ID: " + closestNPC);
            InteractableActivate?.Invoke(this, new InteractableActivateEventArgs { npcID = closestNPC });
        }

    }

    private void OnDestroy()
    {
        gameInput.OnInteractAction -= GameInput_OnInteractAction;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }


    public string closestInteractableID()
    {
        InteractableObject[] interactableObjects = FindObjectsOfType<InteractableObject>();
        float closestDistance = Mathf.Infinity;
        string closestInteractableID = null;

        foreach (InteractableObject interactableObject in interactableObjects)
        {
            if (!(interactableObject.scriptableInteractable.interactionRadius <= (transform.position-interactableObject.GetComponent<Transform>().position).magnitude))
            {
                //Debug.Log("PlayerInteract: NPC " + npc.characterID + " is in interaction radius.");
                float distance = Vector3.Distance(transform.position, interactableObject.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractableID = interactableObject.id;
                }
            }
        }
        return closestInteractableID;
    }
}
