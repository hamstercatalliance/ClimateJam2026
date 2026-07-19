using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu()]
public class NonPlayableCharacterSO : InteractableSO
{
    //public string CharacterName;
    //public string CharacterID;
    //public float interactionRadius = 2.0f; 
    public GameObject prefab;
    public Sprite idleSprite;
    public Sprite talkingSprite; // Displays next to dialogue box

    public Vector3 location;
    public Scene scene;

}
