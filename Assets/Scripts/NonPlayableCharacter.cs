using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonPlayableCharacter : MonoBehaviour
{
    public string characterID { get; protected set; }
    public string characterName { get; protected set; }

    public NonPlayableCharacterSO scriptableNPC;

    [SerializeField] private GameInput gameInput;

    private Player player;

    private void Start()
    {
        characterID = scriptableNPC.CharacterID;
        characterName = scriptableNPC.CharacterName;
        transform.position = scriptableNPC.location;
        player = FindFirstObjectByType<Player>();
        player.GetComponent<PlayerInteract>().NPCActivate += PlayerInteract_NPCActivate;
        //player = FindFirstObjectByType<Player>();
        Init();
    }

    private void PlayerInteract_NPCActivate(object sender, System.EventArgs e)
    {
        OnTalk();
        Debug.Log("NonPlayableCharacter: Player interacted with NPC " + characterID);
    }

    public abstract void OnTalk();

    public abstract void Init();
}
