using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BadEnd : Slideshow
{
    private const string MENU_SCENE = "Menu";
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
    }
    protected override void OnTextFinished()
    {
        SceneManager.LoadScene("Menu");
    }
}
