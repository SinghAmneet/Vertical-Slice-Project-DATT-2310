using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundVisualController : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer backgroundRenderer;

    [Header("Timing")]
    public float initialFullColorTime = 2f;   // how long the background stays normal
    public float fadeDuration = 1f;           // how long the transition takes

    [Header("Background Look")]
    [Range(0f, 1f)] public float targetSaturation = 0.35f; // lower = more gray
    [Range(0f, 1f)] public float targetBrightness = 0.75f; // lower = darker

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
        // Show the background normally for a few seconds first
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
        // Convert to grayscale-ish manually by blending toward luminance
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
