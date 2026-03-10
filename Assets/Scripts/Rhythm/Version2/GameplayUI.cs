using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    [Header("Gameplay Text")]
    public TMP_Text scoreText;
    public TMP_Text multiplierText;
    public TMP_Text rankText;

    [Header("Fade Settings")]
    public float fadeDuration = 0.6f;

    [Header("Score Pulse Settings")]
    public float pulseScaleMultiplier = 1.15f;
    public float pulseDuration = 0.12f;

    [Header("Multiplier Pulse Settings")]
    public float multiplierPulseScaleMultiplier = 1.25f;
    public float multiplierPulseDuration = 0.16f;

    private CanvasGroup canvasGroup;

    private Vector3 scoreBaseScale;
    private Vector3 multiplierBaseScale;

    private Coroutine scorePulseRoutine;
    private Coroutine multiplierPulseRoutine;

    private int lastScore = -1;
    private int lastMultiplier = -1;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;

        if (scoreText != null)
            scoreBaseScale = scoreText.transform.localScale;

        if (multiplierText != null)
            multiplierBaseScale = multiplierText.transform.localScale;

        gameObject.SetActive(false);
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
        StartCoroutine(FadeCanvas(0f, 1f, fadeDuration));
    }

    public void HideUI()
    {
        StartCoroutine(HideUIRoutine());
    }

    IEnumerator HideUIRoutine()
    {
        yield return StartCoroutine(FadeCanvas(1f, 0f, fadeDuration));
        gameObject.SetActive(false);
    }

    IEnumerator FadeCanvas(float from, float to, float duration)
    {
        float timer = 0f;

        if (canvasGroup != null)
            canvasGroup.alpha = from;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            if (canvasGroup != null)
                canvasGroup.alpha = Mathf.Lerp(from, to, t);

            yield return null;
        }

        if (canvasGroup != null)
            canvasGroup.alpha = to;
    }

    public void UpdateGameplayUI(int score, int multiplier, string rank)
    {
        if (scoreText != null)
        {
            scoreText.text = "Rhythm Score: " + score.ToString("D6");

            if (score != lastScore && lastScore >= 0)
            {
                if (scorePulseRoutine != null)
                    StopCoroutine(scorePulseRoutine);

                scorePulseRoutine = StartCoroutine(
                    PulseText(scoreText.transform, scoreBaseScale, pulseScaleMultiplier, pulseDuration)
                );
            }
        }

        if (multiplierText != null)
        {
            multiplierText.text = "Multiplier: x" + multiplier.ToString();

            if (multiplier > lastMultiplier && lastMultiplier >= 0)
            {
                if (multiplierPulseRoutine != null)
                    StopCoroutine(multiplierPulseRoutine);

                multiplierPulseRoutine = StartCoroutine(
                    PulseText(multiplierText.transform, multiplierBaseScale, multiplierPulseScaleMultiplier, multiplierPulseDuration)
                );
            }
        }

        if (rankText != null)
            rankText.text = "Dish Rank: " + rank;

        lastScore = score;
        lastMultiplier = multiplier;
    }

    IEnumerator PulseText(Transform target, Vector3 baseScale, float pulseMultiplier, float duration)
    {
        float timer = 0f;
        Vector3 enlargedScale = baseScale * pulseMultiplier;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            if (t < 0.5f)
            {
                float growT = t / 0.5f;
                target.localScale = Vector3.Lerp(baseScale, enlargedScale, growT);
            }
            else
            {
                float shrinkT = (t - 0.5f) / 0.5f;
                target.localScale = Vector3.Lerp(enlargedScale, baseScale, shrinkT);
            }

            yield return null;
        }

        target.localScale = baseScale;
    }
}