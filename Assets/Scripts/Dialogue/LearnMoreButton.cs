using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LearnMoreButton : MonoBehaviour, IPointerClickHandler
{
    private string learnMoreURL;
    public static LearnMoreButton Instance { get; private set; }
    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        ClearLearnMoreURL();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Application.OpenURL(learnMoreURL);
    }

    public void SetLearnMoreURL(string url)
    {
        learnMoreURL = url;
        gameObject.SetActive(!string.IsNullOrEmpty(learnMoreURL));
       // StartCoroutine(BlinkingEffectCoroutine(0.5f));
    }
    public void ClearLearnMoreURL()
    {
        learnMoreURL = null;
        gameObject.SetActive(false);
        StopAllCoroutines();
    }
    public IEnumerator BlinkingEffectCoroutine(float blinkInterval)
    {
        while (true)
        {
            gameObject.SetActive(!gameObject.activeSelf);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
