using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonDeselector : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Deselect());
    }
    public void OnClick()
    {
        StartCoroutine(Deselect());
    }
    private IEnumerator Deselect() {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
        EventSystem.current.currentInputModule.Process();
    }

}
