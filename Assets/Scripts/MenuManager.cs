using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;
    [SerializeField] private GameObject menuUI;
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
}
