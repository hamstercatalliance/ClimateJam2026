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
    private bool isMenuOpen;
    [SerializeField] private OnClickButtonDisplay menuButtonDisplay;
    // Start is called before the first frame update
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
        if (isMenuOpen)
        {
            menuUI.SetActive(false);
            isMenuOpen = false;
            OnMenuClosed?.Invoke(this, EventArgs.Empty);
        }
        else if (!DialogueBox.dialogueActive && !MerchantStore.merchantStoreOpen)
        {
            menuUI.SetActive(true);
            isMenuOpen = true;
            menuButtonDisplay.UpdateButtonGroup();
            menuButtonDisplay.OnClick(menuButtonDisplay.transform.GetChild(0).gameObject); //select the first button in the menu by default
            Display(menuScreens[0]); //display the first screen in the menu by default
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Display(GameObject gameObject)
    {
        foreach (GameObject screen in menuScreens)
        {
            screen.SetActive(false);
        }
        ItemDisplay.Instance.SetStayVisible(false);
        gameObject.SetActive(true);
    }
}
