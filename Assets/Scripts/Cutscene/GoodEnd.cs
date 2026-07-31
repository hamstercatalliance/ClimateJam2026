using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GoodEnd : Slideshow
{
    private int slide2TextIndexTrigger = 3;

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
        else if (currentTextIndex == slide2TextIndexTrigger)
        {
            ShowNextSlide();
        }
    }
    protected override void OnTextFinished()
    {
        // Load the Menu scene after the slideshow is finished
        SceneManager.LoadScene("Menu");
    }
}
