using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GameItemSO : ScriptableObject
{
    public GameObject worldSpacePrefab;
    public Sprite inventorySprite;
    public string itemName;
}
