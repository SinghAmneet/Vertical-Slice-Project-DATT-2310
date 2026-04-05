using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static string TargetSceneName;

    public static void LoadScene(string targetSceneName)
    {
        TargetSceneName = targetSceneName;
        SceneManager.LoadScene("LoadingScene");
    }
}
