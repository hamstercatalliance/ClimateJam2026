using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }
    public void ContinueGame()
    {
        DataPersistenceManager.Instance.LoadPlayerData();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
