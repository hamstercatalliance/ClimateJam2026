using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class InteractableSO : ScriptableObject
{

    public float interactionRadius = 3.0f;
    public string interactableName;
    public string interactableId;

    //public GameObject prefab;
    //public Sprite idleSprite;
    //public Sprite talkingSprite; // Displays next to dialogue box

    //public Vector3 location;

}
