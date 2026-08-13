using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics; //stack trace
using System.Reflection;

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
        StackTrace trace = new StackTrace(1, true);
        StackFrame callerFrame = trace.GetFrame(0);

        MethodBase callerMethod = callerFrame.GetMethod();
        string callerClass = callerMethod.DeclaringType != null ? callerMethod.DeclaringType.FullName : "(Unknown Class)";
        string callerName = callerMethod.Name;

        UnityEngine.Debug.LogWarning(
            $"MyButtonFunction was called by: {callerClass}.{callerName} (at {callerFrame.GetFileName()}:{callerFrame.GetFileLineNumber()})"
        );
        
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
