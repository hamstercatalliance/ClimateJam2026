using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class TriggerSceneLoader : SceneLoader
{
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
    public void SceneLoader_OnSceneLoaderCollided(object sender, Player.OnSceneLoaderCollidedEventArgs e)
    {
        //if the scene loader is a trigger object
        if (e.sceneLoaderGameObject != this.gameObject)
        {
            return;
        }
        LoadSceneRoutine();
    }
}
