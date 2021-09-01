using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    private Scene scene;

    private void Start()
    {
        scene = SceneManager.GetSceneByName("SampleScene");
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(scene.name);
    }
}
