using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerVanity : MonoBehaviour
{
    [SerializeField] private Sprite blueBow;
    [SerializeField] private Sprite pinkBow;
    [SerializeField] private Sprite purpleBow;
    [SerializeField] private Sprite blackTie;
    [SerializeField] private Sprite stripedTie;
    [SerializeField] private Sprite dotsTie;
    private const string BLUE_BOW_ID = "item.vanity.bow_blue";
    private const string PINK_BOW_ID = "item.vanity.bow_pink";
    private const string PURPLE_BOW_ID = "item.vanity.bow_purple";
    private const string BLACK_TIE_ID = "item.vanity.tie_black";
    private const string STRIPED_TIE_ID = "item.vanity.tie_stripe";
    private const string RED_TIE_ID = "item.vanity.tie_dots";
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = null;
        VanityManager.Instance.OnVanityItemEquipped += VanityManager_OnVanityItemEquipped;
    }
    private void VanityManager_OnVanityItemEquipped(object sender, VanityManager.OnVanityItemEquippedEventArgs e)
    {
        if (e.equippedItem == null)
        {
            GetComponent<SpriteRenderer>().sprite = null;
            return;
        }
        switch (e.equippedItem.itemID)
        {
            case BLUE_BOW_ID:
                GetComponent<SpriteRenderer>().sprite = blueBow;
                break;
            case PINK_BOW_ID:
                GetComponent<SpriteRenderer>().sprite = pinkBow;
                break;
            case PURPLE_BOW_ID:
                GetComponent<SpriteRenderer>().sprite = purpleBow;
                break;
            case BLACK_TIE_ID:
                GetComponent<SpriteRenderer>().sprite = blackTie;
                break;
            case STRIPED_TIE_ID:
                GetComponent<SpriteRenderer>().sprite = stripedTie;
                break;
            case RED_TIE_ID:
                GetComponent<SpriteRenderer>().sprite = dotsTie;
                break;
            default:
                Debug.LogWarning("Unknown vanity item equipped: " + e.equippedItem.itemID);
                break;
        }
    }
}
