using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject[] IHasPersistentDataGameObjects;
    public static event EventHandler OnSceneTransition;
    private void Start()
    {
        Player.Instance.OnSceneLoaderCollided += SceneLoader_OnSceneLoaderCollided;

    }
    private void OnDestroy()
    {
        if (Player.Instance != null)
        {
            Player.Instance.OnSceneLoaderCollided -= SceneLoader_OnSceneLoaderCollided;
        }
    }
    public void SceneLoader_OnSceneLoaderCollided(object sender, EventArgs e)
    {
        OnSceneTransition?.Invoke(this, EventArgs.Empty);
        GameData.Instance.HasLoadedRunData = true;
        StartCoroutine(WaitForAllDataToBeWrittenAndLoadScene());
    }

    private IEnumerator WaitForAllDataToBeWrittenAndLoadScene()
    {
        Debug.Log("Waiting for condition...");
        yield return new WaitUntil(() => CheckAllDataWritten());
        Debug.Log("Condition met! Resuming coroutine.");
        SceneManager.LoadScene(sceneName);
    }
    private bool CheckAllDataWritten()
    {
        foreach (GameObject persistentDataGameObject in IHasPersistentDataGameObjects)
        {
            MonoBehaviour[] components = persistentDataGameObject.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour component in components)
            {
                if (component is IHasPersistentData persistentData)
                {
                    Debug.Log("Data for: " + persistentDataGameObject.name + " - Successfully Written: " + persistentData.DataSuccessfullyWritten);
                    if (!persistentData.DataSuccessfullyWritten)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

}
