using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private TMP_Text tipText;
    [SerializeField] private CanvasGroup fadeGroup;

    [Header("Timing")]
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float minLoadTime = 5f;
    [SerializeField] private float maxLoadTime = 7f;

    [Header("Tips")]
    [TextArea]
    [SerializeField] private string[] loadingTips = {};

    private void Start()
    {
        StartCoroutine(BeginLoadingFlow());
    }

    private IEnumerator BeginLoadingFlow()
    {
        // Fade in from black
        if (fadeGroup != null)
        {
            fadeGroup.alpha = 1f;
            yield return StartCoroutine(Fade(1f, 0f));
        }

        SetRandomTip();
        StartCoroutine(AnimateLoadingText());

        yield return StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        string sceneToLoad = SceneLoader.TargetSceneName;

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("No target scene set.");
            yield break;
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        float timer = 0f;
        float targetTime = Random.Range(minLoadTime, maxLoadTime);

        bool sceneReady = false;

        while (!operation.isDone)
        {
            timer += Time.unscaledDeltaTime;

            if (operation.progress >= 0.9f)
            {
                sceneReady = true;
            }

            if (sceneReady && timer >= targetTime)
            {
                if (loadingText != null)
                {
                    loadingText.text = "Starting...";
                }

                yield return new WaitForSecondsRealtime(0.5f);

                // Fade out to black
                if (fadeGroup != null)
                {
                    yield return StartCoroutine(Fade(0f, 1f));
                }

                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private IEnumerator AnimateLoadingText()
    {
        if (loadingText == null) yield break;

        string baseText = "Loading";
        int dots = 0;

        while (true)
        {
            dots = (dots + 1) % 4;
            loadingText.text = baseText + new string('.', dots);
            yield return new WaitForSecondsRealtime(0.4f);
        }
    }

    private void SetRandomTip()
    {
        if (tipText == null || loadingTips.Length == 0) return;

        int index = Random.Range(0, loadingTips.Length);
        tipText.text = loadingTips[index];
    }

    private IEnumerator Fade(float start, float end)
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float value = Mathf.Lerp(start, end, t / fadeDuration);

            if (fadeGroup != null)
            {
                fadeGroup.alpha = value;
            }

            yield return null;
        }

        if (fadeGroup != null)
        {
            fadeGroup.alpha = end;
        }
    }
}