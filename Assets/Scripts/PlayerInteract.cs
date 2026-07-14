using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerInteract : MonoBehaviour
{
    public EventHandler NPCActivate;
    [SerializeField] private GameInput gameInput;
    public class NPCActivateEventArgs : EventArgs
    {
        public string npcID;

    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        string closestNPC = this.closestNPCID();
        //Debug.Log("PlayerInteract: Interact action triggered, NPC = " + closestNPC);
        if (!DialogueBox.dialogueActive && closestNPC != null)
        {
          //Debug.Log("PlayerInteract: Interact action triggered, closest NPC ID: " + closestNPC);
            NPCActivate?.Invoke(this, new NPCActivateEventArgs { npcID = closestNPC });
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


    public string closestNPCID()
    {
        NonPlayableCharacter[] npcs = FindObjectsOfType<NonPlayableCharacter>();
        float closestDistance = Mathf.Infinity;
        string closestNPCID = null;

        foreach (NonPlayableCharacter npc in npcs)
        {
            if (!(npc.scriptableNPC.interactionRadius <= (transform.position-npc.GetComponent<Transform>().position).magnitude))
            {
                //Debug.Log("PlayerInteract: NPC " + npc.characterID + " is in interaction radius.");
                float distance = Vector3.Distance(transform.position, npc.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestNPCID = npc.characterID;
                }
            }
        }
        return closestNPCID;
    }
}
