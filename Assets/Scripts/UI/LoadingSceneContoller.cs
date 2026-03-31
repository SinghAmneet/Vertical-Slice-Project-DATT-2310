using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private Slider progressBar;

    private void Start()
    {
        StartCoroutine(LoadTargetScene());
    }

    private IEnumerator LoadTargetScene()
    {
        string sceneToLoad = SceneLoader.TargetSceneName;

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("No target scene was set before loading LoadingScene.");
            yield break;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            if (loadingText != null)
            {
                loadingText.text = "Loading... " + Mathf.RoundToInt(progress * 100f) + "%";
            }

            if (operation.progress >= 0.9f)
            {
                if (loadingText != null)
                {
                    loadingText.text = "Ready...";
                }

                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
