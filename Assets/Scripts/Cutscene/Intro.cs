using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class Intro : Slideshow
{
    [SerializeField] private DayCountdown dayCountdown;
    private int slide2TextIndexTrigger = 1;
    private int slide3TextIndexTrigger = 4;
    private int slide4TextIndexTrigger = 7;
    [SerializeField] private string HOME_SCENE = "Home";
    private bool finalClicked = false;
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
            if (finalClicked)
            {
                return; //prevent multiple clicks from triggering multiple scene loads
            }
            finalClicked = true;
            SceneManager.LoadScene(HOME_SCENE);
        }
        else if (currentTextIndex == slide2TextIndexTrigger)
        {
            ShowNextSlide();
        }
        else if (currentTextIndex == slide3TextIndexTrigger)
        {
            ShowNextSlide();
        }
        else if (currentTextIndex == slide4TextIndexTrigger)
        {
            ShowNextSlide();
        }
        else if (currentTextIndex > slide4TextIndexTrigger && currentSlideIndex < slides.Length)
        {
            ShowNextSlide();
        }
    }
}
