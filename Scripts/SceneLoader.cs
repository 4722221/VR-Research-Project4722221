using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ← これがシーン移動に必要です

public class SceneLoader : MonoBehaviour
{
    // string sceneName には、移動したいシーンの名前が入ります
    public void LoadSceneByName(string sceneName)
    {
        // sceneName の名前のシーンを読み込みます
        SceneManager.LoadScene(sceneName);
    }
}
