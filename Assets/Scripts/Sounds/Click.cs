using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class Click : MonoBehaviour, IPointerClickHandler
{
    public static event EventHandler OnClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke(this, EventArgs.Empty);
    }
}
