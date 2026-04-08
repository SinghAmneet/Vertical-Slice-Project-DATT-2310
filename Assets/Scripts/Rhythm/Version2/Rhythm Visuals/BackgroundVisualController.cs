using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundVisualController : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer backgroundRenderer;

    [Header("Timing")]
    public float initialFullColorTime;   // how long the background stays normal
    public float fadeDuration;           // how long the transition takes

    [Header("Background Look")]
    [Range(0f, 1f)] public float targetSaturation; // lower = more gray
    [Range(0f, 1f)] public float targetBrightness; // lower = darker

    private Color originalColor;
    private Color fadedColor;

    void Awake()
    {
        if (backgroundRenderer == null)
            backgroundRenderer = GetComponent<SpriteRenderer>();

        if (backgroundRenderer != null)
        {
            originalColor = backgroundRenderer.color;
            fadedColor = GetFadedColor(originalColor);
        }
    }

    public void StartCountdownFade()
    {
        StartCoroutine(FadeBackgroundRoutine());
    }

    IEnumerator FadeBackgroundRoutine()
    {
        //Show the background normally for a few seconds first
        yield return new WaitForSeconds(initialFullColorTime);

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;

            if (backgroundRenderer != null)
                backgroundRenderer.color = Color.Lerp(originalColor, fadedColor, t);

            yield return null;
        }

        if (backgroundRenderer != null)
            backgroundRenderer.color = fadedColor;
    }

    Color GetFadedColor(Color source)
    {
        float gray = source.r * 0.299f + source.g * 0.587f + source.b * 0.114f;

        Color grayColor = new Color(gray, gray, gray, source.a);

        // Blend original color toward grayscale
        Color desaturated = Color.Lerp(grayColor, source, targetSaturation);

        // Darken slightly
        desaturated.r *= targetBrightness;
        desaturated.g *= targetBrightness;
        desaturated.b *= targetBrightness;

        return desaturated;
    }
}
