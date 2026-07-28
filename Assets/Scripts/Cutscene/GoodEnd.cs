using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodEnd : Slideshow
{
    private int slide2TextIndexTrigger = 3;

    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        int previousIndex = currentTextIndex;

        base.OnPointerClick(eventData);

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
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
