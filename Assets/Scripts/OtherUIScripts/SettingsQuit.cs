using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SettingsQuit : MonoBehaviour
{
    [SerializeField] private GameObject quitPopup;
    private const string MENU_SCENE_NAME = "Menu"; 
    // Start is called before the first frame update
    void Start()
    {
        quitPopup.SetActive(false);
    }
    public void ShowQuitPopup()
    {
        quitPopup.SetActive(true);
    }
    public void HideQuitPopup()
    {
        quitPopup.SetActive(false);
    }
    public void QuitGame()
    {
        SceneManager.LoadScene(MENU_SCENE_NAME);
    }
}
