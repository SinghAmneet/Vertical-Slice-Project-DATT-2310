using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayUI : MonoBehaviour
{
    [Header("References")]
    public SongController songController;

    [Header("Gameplay Text")]
    public TMP_Text scoreText;
    public TMP_Text multiplierText;
    public TMP_Text rankText;
    public TMP_Text timerText;

    [Header("Fade Settings")]
    public float fadeDuration;

    [Header("Score Pulse Settings")]
    public float pulseScaleMultiplier;
    public float pulseDuration;

    [Header("Multiplier Pulse Settings")]
    public float multiplierPulseScaleMultiplier;
    public float multiplierPulseDuration;

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

    void Update()
    {
        UpdateTimerUI();
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
        {
            string coloredRank = GetColoredRank(rank);
            rankText.text = "Dish Rank: " + coloredRank;
        }

        lastScore = score;
        lastMultiplier = multiplier;
    }

    void UpdateTimerUI()
    {
        if (timerText == null || songController == null)
            return;

        double songTime = songController.GetSongTime();
        double remainingTime;

        if (songTime < 0)
            remainingTime = songController.songLength;
        else
            remainingTime = songController.songLength - songTime;

        if (remainingTime < 0)
            remainingTime = 0;

        int minutes = Mathf.FloorToInt((float)remainingTime / 60f);
        int seconds = Mathf.FloorToInt((float)remainingTime % 60f);

        timerText.text = "Time Left: " + minutes.ToString("00") + ":" + seconds.ToString("00");
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

    string GetColoredRank(string rank)
    {
        switch (rank)
        {
            case "S":
                return "<b><color=#FFD700>S</color></b>";
            case "A":
                return "<b><color=#7CFF7C>A</color></b>";
            case "B":
                return "<b><color=#66CCFF>B</color></b>";
            case "C":
                return "<b><color=#FFD966>C</color></b>";
            case "D":
                return "<b><color=#FF9E66>D</color></b>";
            case "F":
                return "<b><color=#FF6B6B>F</color></b>";
            default:
                return rank;
        }
    }
}
