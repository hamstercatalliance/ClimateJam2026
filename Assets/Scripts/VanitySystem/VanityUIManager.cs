using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VanityUIManager : MonoBehaviour
{
    [SerializeField] private GameObject vanityItemButtonPrefab;
    [SerializeField] private Transform contentParent;
    public static VanityUIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void CreateVanityItemButton(GameItemSO vanityItem)
    {
        GameObject buttonGO = Instantiate(vanityItemButtonPrefab, contentParent);
        VanitySlotUI buttonUI = buttonGO.GetComponent<VanitySlotUI>();
        buttonUI.SetItem(vanityItem);
        buttonGO.SetActive(true);
    }
}
