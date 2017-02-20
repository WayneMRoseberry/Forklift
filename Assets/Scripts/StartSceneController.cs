using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneController : MonoBehaviour {

    public static List<string> completedScenes = new List<string>();

    public void LoadFirstScene()
    {
        SceneManager.LoadScene(1);
    }
}
