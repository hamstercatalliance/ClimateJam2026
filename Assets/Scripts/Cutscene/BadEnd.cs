using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BadEnd : Slideshow
{
    private const string MENU_SCENE = "Menu";
    protected override void OnPlayerInput()
    {
        int previousIndex = currentTextIndex;

        base.OnPlayerInput();

        if (previousIndex == currentTextIndex)
        {   
            return;
        }

        if (currentTextIndex >= slideTexts.Length)
        {
            OnTextFinished();
        }
    }
    protected override void OnTextFinished()
    {
        SceneManager.LoadScene("Menu");
    }
}
