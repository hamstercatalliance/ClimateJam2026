using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject instructionsPrefab;
    [SerializeField] private CanvasGroup buttons;
    private string HOME_SCENE_NAME = "Home";
    private string INTRO_SCENE_NAME = "Intro";
    // Start is called before the first frame update
    void Start()
    {
        instructionsPrefab.SetActive(false);
        buttons.interactable = false;
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData && GameData.Instance.HasCompletedFirstDay)
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
        buttons.interactable = true;
    }
    public void StartNewGame()
    {
        DataPersistenceManager.Instance.ClearGame();
        StartCoroutine(PauseAndLoadScene(INTRO_SCENE_NAME));
    }
    public void ContinueGame()
    {
        DataPersistenceManager.Instance.LoadPlayerData();
        StartCoroutine(PauseAndLoadScene(HOME_SCENE_NAME));
    }
    public void ExitGame()
    {
        StartCoroutine(PauseAndQuit());
    }

    private IEnumerator PauseAndLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(0.4f);
        SceneManager.LoadScene(sceneName);
    }


    private IEnumerator PauseAndQuit()
    {
        yield return new WaitForSeconds(0.4f);
        Application.Quit();
    }

    public void OpenInstructions()
    {
        instructionsPrefab.SetActive(true);
    }
    public void CloseInstructions()
    {
        instructionsPrefab.SetActive(false);
    }
}
