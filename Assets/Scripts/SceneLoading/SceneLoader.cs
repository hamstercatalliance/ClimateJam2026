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
    [SerializeField] private Animator fadeToBlack;

    public void LoadSceneRoutine()
    {
        Debug.Log("=== SceneLoader.LoadSceneRoutine() called ===");
        Debug.Log($"SceneLoader - Scene to load: '{sceneName}'");
        //this makes it so that a scene loader doesnt neccesarily have ot be a collidable object
        OnSceneTransition?.Invoke(this, EventArgs.Empty);
        GameData.Instance.HasLoadedRunData = true;
        Debug.Log("SceneLoader - Starting WaitForAllDataToBeWrittenAndLoadScene coroutine");
        StartCoroutine(WaitForAllDataToBeWrittenAndLoadScene());
    }
    private IEnumerator WaitForAllDataToBeWrittenAndLoadScene()
    {
        yield return new WaitUntil(() => CheckAllDataWritten());
        if (fadeToBlack != null)
        {
            fadeToBlack.Play("BasicFade");
            yield return null;
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            while (fadeToBlack.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || operation.progress < 0.9f)
            {
                yield return null;
            }
            operation.allowSceneActivation = true;
        }
        else //fallback if no fade 
        {
            SceneManager.LoadScene(sceneName);
        }
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
