using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILoader : MonoBehaviour
{
    private void Awake()
    {
        if(SceneManager.GetSceneByName("UI").isLoaded == false)
        {
            SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.UnloadSceneAsync("UI");
        }
    }
}
