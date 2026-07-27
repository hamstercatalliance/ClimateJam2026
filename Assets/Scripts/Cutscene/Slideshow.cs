using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
public abstract class Slideshow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject slideshow;
    [SerializeField] private GameObject textBox; //In case you want to move the text box around
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Sprite[] slides;
    [SerializeField] private string[] slideTexts;
    private int currentSlideIndex = 0;
    private int currentTextIndex = 0;
    public void ShowNextSlide()
    {
        slideshow.GetComponent<SpriteRenderer>().sprite = slides[currentSlideIndex];
        currentSlideIndex++;
    }
    public void ShowNextText()
    {
        text.text = slideTexts[currentTextIndex];
        currentTextIndex++;
    }
    public virtual void OnPointerClick(PointerEventData eventData) //customize in derived classes
    {
        
    }
}
