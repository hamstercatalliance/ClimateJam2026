using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    private string HOME_SCENE_NAME = "Home";
    private string INTRO_SCENE_NAME = "Intro";
    // Start is called before the first frame update
    void Start()
    {
        if (GameData.Instance != null && GameData.Instance.HasLoadedRunData && GameData.Instance.HasCompletedFirstDay)
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }
    public void StartNewGame()
    {
        DataPersistenceManager.Instance.ClearGame();
        SceneManager.LoadScene(INTRO_SCENE_NAME);
    }
    public void ContinueGame()
    {
        DataPersistenceManager.Instance.LoadPlayerData();
        SceneManager.LoadScene(HOME_SCENE_NAME);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
