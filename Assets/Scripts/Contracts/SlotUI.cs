using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public abstract class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //hover
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //hover exit
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //click
    }
}
