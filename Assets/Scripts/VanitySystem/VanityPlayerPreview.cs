using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class VanityPlayerPreview : MonoBehaviour
{
    public static VanityPlayerPreview Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    [SerializeField] private GameObject accessoryPreviewObject;
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
    private bool stayVisible = false;
    public void SetStayVisible(bool value)
    {
        stayVisible = value;
    }
    private void OnEnable()
    {
        if (VanityManager.Instance != null)
        {
            ShowEquippedOrNothing();
        }
    }
    public void ShowEquippedOrNothing()
    {
        Debug.Log(VanityManager.Instance);
        Debug.Log(VanityManager.Instance.equipedVanityItem);
        GameItemSO equipped = VanityManager.Instance.equipedVanityItem;
        if (equipped != null)
        {
            accessoryPreviewObject.SetActive(true);
            switch (equipped.itemID)
            {
                case BLUE_BOW_ID:
                    accessoryPreviewObject.GetComponent<Image>().sprite = blueBow;
                    break;
                case PINK_BOW_ID:
                    accessoryPreviewObject.GetComponent<Image>().sprite = pinkBow;
                    break;
                case PURPLE_BOW_ID:
                    accessoryPreviewObject.GetComponent<Image>().sprite = purpleBow;
                    break;
                case BLACK_TIE_ID:
                    accessoryPreviewObject.GetComponent<Image>().sprite = blackTie;
                    break;
                case STRIPED_TIE_ID:
                    accessoryPreviewObject.GetComponent<Image>().sprite = stripedTie;
                    break;
                case RED_TIE_ID:
                    accessoryPreviewObject.GetComponent<Image>().sprite = dotsTie;
                    break;
                default:
                    Debug.LogWarning("Unknown vanity item equipped: " + equipped.itemID);
                    break;
            }
        }
        else
        {
            accessoryPreviewObject.SetActive(false);
        }
    }
    void Start()
    {
        VanitySlotUI.OnVanitySlotHovered += VanitySlotUI_OnVanitySlotHovered;
        VanitySlotUI.OnVanitySlotHoverExit += VanitySlotUI_OnVanitySlotHoverExit;

        ShowEquippedOrNothing();
    }
    private void OnDestroy()
    {
        VanitySlotUI.OnVanitySlotHovered -= VanitySlotUI_OnVanitySlotHovered;
        VanitySlotUI.OnVanitySlotHoverExit -= VanitySlotUI_OnVanitySlotHoverExit;
    }
    private void VanitySlotUI_OnVanitySlotHovered(object sender, VanitySlotUI.OnSlotHoveredEventArgs e)
    {
        GameItemSO item = e.vanityItemSO;
        if (stayVisible == false)
        {
            accessoryPreviewObject.SetActive(true);
            switch (item.itemID)
            {
                case BLUE_BOW_ID:
                    accessoryPreviewObject.GetComponent<Image>().sprite = blueBow;
                    break;
                case PINK_BOW_ID:
                    accessoryPreviewObject.GetComponent<Image>().sprite = pinkBow;
                    break;
                case PURPLE_BOW_ID:
                    accessoryPreviewObject.GetComponent<Image>().sprite = purpleBow;
                    break;
                case BLACK_TIE_ID:
                    accessoryPreviewObject.GetComponent<Image>().sprite = blackTie;
                    break;
                case STRIPED_TIE_ID:
                    accessoryPreviewObject.GetComponent<Image>().sprite = stripedTie;
                    break;
                case RED_TIE_ID:
                    accessoryPreviewObject.GetComponent<Image>().sprite = dotsTie;
                    break;
                default:
                    Debug.LogWarning("Unknown vanity item hovered: " + item.itemID);
                    break;
            }
        }
    }
    private void VanitySlotUI_OnVanitySlotHoverExit(object sender, EventArgs e)
    {
        if (stayVisible == false)
        {
            ShowEquippedOrNothing();
        }
    }
    public void ClearPreview()
    {
        accessoryPreviewObject.SetActive(false);
    }
}
