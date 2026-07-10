using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonPlayableCharacter : MonoBehaviour
{
    public string characterID { get; private set; }
    public string characterName { get; private set; }

    [SerializeField] protected NonPlayableCharacterSO scriptableNPC;

    private void Start()
    {
        transform.position = scriptableNPC.location;
        Init();
    }



    public abstract void OnTalk();

    public abstract void Init();
}
