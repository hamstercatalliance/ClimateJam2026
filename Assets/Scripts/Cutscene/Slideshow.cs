using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public abstract class Slideshow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameInput gameInput;
    
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
    private CutsceneInput cutsceneInput;
    private void Awake()
    {
        cutsceneInput = FindObjectOfType<CutsceneInput>();
        cutsceneInput.OnCutsceneProceed += CutsceneInput_OnCutsceneProceed;
    }
    private void OnDestroy()
    {
        cutsceneInput.OnCutsceneProceed -= CutsceneInput_OnCutsceneProceed;
    }
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
    public void OnPointerClick(PointerEventData eventData)
    {
        OnPlayerInput();
    }
    private void CutsceneInput_OnCutsceneProceed(object sender, EventArgs e)
    {
        OnPlayerInput();
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
    protected virtual void OnPlayerInput()
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
    protected virtual void OnTextFinished() {}

}
