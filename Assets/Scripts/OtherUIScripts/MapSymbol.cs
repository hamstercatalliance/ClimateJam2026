using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MapSymbol : SlotUI
{
    private Vector3 originalScale;
    [SerializeField] private float scaleMultiplier = 1.1f;
    [SerializeField] private GameObject symbolText;
    
    private void Start()
    {
        originalScale = transform.localScale;
        symbolText.SetActive(false);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        gameObject.transform.localScale = originalScale * scaleMultiplier;
        symbolText.SetActive(true);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        gameObject.transform.localScale = originalScale;
        symbolText.SetActive(false);
    }
}
