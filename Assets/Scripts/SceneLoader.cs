using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public static event EventHandler OnSceneTransition;
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            OnSceneTransition?.Invoke(this, EventArgs.Empty);
            SceneManager.LoadScene(sceneName);
        }
    }
}
