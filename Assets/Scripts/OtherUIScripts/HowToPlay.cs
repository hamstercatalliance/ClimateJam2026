using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    [SerializeField] private GameObject helpButton;
    [SerializeField] private GameObject instructionsPrefab;
    public static HowToPlay Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        helpButton.SetActive(true);
        instructionsPrefab.SetActive(false);
    }
    public void OpenInstructions()
    {
        helpButton.SetActive(false);
        instructionsPrefab.SetActive(true);
    }
    public void CloseInstructions()
    {
        helpButton.SetActive(true);
        instructionsPrefab.SetActive(false);
    }
    public void HideHelpButton()
    {
        helpButton.SetActive(false);
    }
}
