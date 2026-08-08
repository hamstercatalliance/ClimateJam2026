using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private List<GameObject> menuScreens;
    public event EventHandler OnMenuClosed;
    public event EventHandler OnMenuOpened;
    private bool isMenuOpen;
    [SerializeField] private OnClickButtonDisplay menuButtonDisplay;
    
    void Start()
    {
        gameInput.OnMenuAction += GameInput_OnMenuAction;
        menuUI.SetActive(false);
        isMenuOpen = false;
    }
    private void OnDestroy()
    {
        gameInput.OnMenuAction -= GameInput_OnMenuAction;
    }
    private void GameInput_OnMenuAction(object sender, System.EventArgs e)
    {
        ToggleMenu();
    }
    public void ToggleMenu()
    {
        if (isMenuOpen)
        {
            menuUI.SetActive(false);
            isMenuOpen = false;

            OnMenuClosed?.Invoke(this, EventArgs.Empty);
        }
        else if (!DialogueBox.dialogueActive && !MerchantStore.merchantStoreOpen && !BoardManager.jobBoardActive)
        {
            menuUI.SetActive(true);
            isMenuOpen = true;
            menuButtonDisplay.UpdateButtonGroup();
            menuButtonDisplay.OnClick(menuButtonDisplay.transform.GetChild(0).gameObject); //select the first button in the menu by default
            Display(menuScreens[0]); //display the first screen in the menu by default

            OnMenuOpened?.Invoke(this, EventArgs.Empty);
            
            QuestNotifyManager.Instance.HideQuestNotification(); //hide the quest notification when opening the menu
        }
    }
    public void Display(GameObject gameObject)
    {
        foreach (GameObject screen in menuScreens)
        {
            screen.SetActive(false);
        }
        if (ItemDisplay.Instance != null)
        {
            ItemDisplay.Instance.SetStayVisible(false);
        }
        if (DiscardUIManager.Instance != null)
        {
            DiscardUIManager.Instance.ClearGameItem();
        }

        gameObject.SetActive(true);
    }
}
