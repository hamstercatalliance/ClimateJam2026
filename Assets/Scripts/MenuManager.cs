using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private List<GameObject> menuScreens;
    private bool isMenuOpen;
    // Start is called before the first frame update
    void Start()
    {
        gameInput.OnMenuAction += GameInput_OnMenuAction;
        menuUI.SetActive(false);
        isMenuOpen = false;
    }
    private void GameInput_OnMenuAction(object sender, System.EventArgs e)
    {
        if (isMenuOpen)
        {
            menuUI.SetActive(false);
            isMenuOpen = false;
        }
        else
        {
            menuUI.SetActive(true);
            isMenuOpen = true;
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
        gameObject.SetActive(true);
    }
}
