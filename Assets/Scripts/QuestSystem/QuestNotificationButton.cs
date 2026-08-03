using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class QuestNotificationButton : SlotUI
{
     private Vector3 originalScale;
    [SerializeField] private float scaleMultiplier = 1.2f;
    private void Start()
    {
        originalScale = transform.localScale;
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * scaleMultiplier;
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
