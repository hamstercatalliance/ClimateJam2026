using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;
public class LinkOpener : MonoBehaviour, IPointerClickHandler
{
    public static event EventHandler OnLinkClicked;
    public void OnPointerClick(PointerEventData eventData)
    {
        OnLinkClicked?.Invoke(this, EventArgs.Empty);
    }
    public static void OpenLink(string url)
    {
        Application.OpenURL(url);
    }
}
