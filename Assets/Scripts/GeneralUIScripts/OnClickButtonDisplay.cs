using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OnClickButtonDisplay : MonoBehaviour
{
    [SerializeField] private Color originalColor;
    [SerializeField] private Color onClickColor;
    private List<GameObject> buttonGroup; //this script is meant to be attached to a container w al the buttons as children
    private GameObject selectedButton;
    [SerializeField] private bool selectedButtonIsDeselectable; //if true, clicking the selected button again will deselect it. if false, clicking the selected button again will do nothing
    private void OnEnable()
    {
        Debug.Log("Starting OnClickButtonDisplay");
        selectedButton = null;
        buttonGroup = new List<GameObject>();
        UpdateButtonGroup();
        DeselectAllButtons();
    }
    private void OnDisable()
    {
        DeselectAllButtons();
    }
    public void OnClick(GameObject button)
    {
        DeselectAllButtons();
        if (selectedButton == button && selectedButtonIsDeselectable)
        {
            //Debug.Log("Deselecting button: " + button.name);
            selectedButton = null;
            return;
        }
        //Debug.Log("Selecting button: " + button.name);
        selectedButton = button;
        button.GetComponent<Image>().color = onClickColor;
    }
    // private void EnsureInitialized()
    // {
    //     if (buttonGroup == null)
    //     {
    //         buttonGroup = new List<GameObject>();
    //         foreach (Transform child in transform)
    //         {
    //             buttonGroup.Add(child.gameObject);
    //         }
    //     }
    // }
    private void DeselectAllButtons()
    {
        //EnsureInitialized();
        foreach (GameObject button in buttonGroup)
        {
            Debug.Log("Deselecting button: " + button.name);
            Deselect(button);
        }
    }
    public void Deselect(GameObject button)
    {
        button.GetComponent<Image>().color = originalColor;
        //Debug.Log($"{button.name} color set to {originalColor}, actual now: {button.GetComponent<Image>().color}");
    }
    public void UpdateButtonGroup() 
    {
        //for dynamically generated buttons, call this function to update the button group list
        //EnsureInitialized();
        buttonGroup.Clear();
        foreach (Transform child in transform)
        {
            buttonGroup.Add(child.gameObject);
            child.GetComponent<Image>().color = (child.gameObject == selectedButton) ? onClickColor : originalColor;
        }
    }
}
