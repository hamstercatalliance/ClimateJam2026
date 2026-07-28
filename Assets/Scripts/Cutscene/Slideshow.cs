using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public abstract class Slideshow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] protected GameObject slideshow;
    [SerializeField] protected GameObject textBox; //In case you want to move the text box around
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected Sprite[] slides;
    [SerializeField] protected string[] slideTexts;
    [SerializeField] protected float textSpeed = 0.02f;
    protected int currentSlideIndex = 0;
    protected int currentTextIndex = 0;
    protected bool isTyping = false;
    protected Coroutine typingCoroutine;
    public void ShowNextSlide()
    {
        if (currentSlideIndex >= slides.Length)
        {
            Debug.LogWarning("ShowNextSlide: No more slides to show.");
            return;
        }

        slideshow.GetComponent<Image>().sprite = slides[currentSlideIndex];
        currentSlideIndex++;
    }
    public void ShowNextText()
    {
        if (currentTextIndex >= slideTexts.Length)
        {
            Debug.LogWarning("ShowNextText: No more texts to show.");
            return;
        }

        typingCoroutine = StartCoroutine(TypeWriterEffect(slideTexts[currentTextIndex], textSpeed));
    }
    private void Start()
    {
        ShowNextSlide();
        if (slideTexts.Length > 0)
        {
            ShowNextText();
        }
    }
   public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);

            isTyping = false;
            text.text = slideTexts[currentTextIndex];

            return;
        }

        currentTextIndex++;

        if (currentTextIndex < slideTexts.Length)
        {
            ShowNextText();
        }
    }
    private IEnumerator TypeWriterEffect(string fullText, float delay)
    {
        isTyping = true;
        text.text = "";
        foreach (char c in fullText)
        {
            text.text += c;
            yield return new WaitForSeconds(delay);
        }
        isTyping = false;
    }
    protected virtual void OnTextFinished() {}

}
