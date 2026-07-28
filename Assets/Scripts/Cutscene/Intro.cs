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
    public override void OnPointerClick(PointerEventData eventData)
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
    protected override void OnTextFinished()
    {
        if (currentTextIndex >= slideTexts.Length)
        {
            StartCoroutine(IntroFinishedRoutine());
        }
    }
    private IEnumerator IntroFinishedRoutine()
    {
        yield return dayCountdown.ShowCountdownCoroutine();
        SceneManager.LoadScene(HOME_SCENE);
    }

}
