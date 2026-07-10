using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; 
public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private GameObject icon;
    [SerializeField] private TextMeshProUGUI amountText;
    public void SetIcon(Sprite sprite)
    {
        Debug.Log("Setting icon for slot");
        //set the sprite (image for UI)
        icon.GetComponent<Image>().sprite = sprite;
        icon.SetActive(true); //EVENTUALLY HANDLE THIS ELSEWHERE
    }
    public void ClearSlot()
    {
        //clear the slot
        icon.SetActive(false); //EVENTUALLY HANDLE THIS ELSEWHERE
        amountText.text = "";
    }
    public void SetAmount(int amount)
    {
        amountText.text = amount.ToString();
        Debug.Log("Setting amount for slot");
    }
    public void ShowChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
