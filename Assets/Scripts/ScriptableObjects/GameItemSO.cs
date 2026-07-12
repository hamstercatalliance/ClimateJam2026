using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class GameItemSO : ScriptableObject
{
    public GameObject worldSpacePrefab;
    public Sprite inventorySprite;
    public string itemName;
    public string itemDescription;
    public string sourceLink;
    public int cost;
}
