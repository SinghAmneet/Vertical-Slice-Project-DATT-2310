using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCirclePulse : MonoBehaviour
{
    [Header("References")]
    public SongController songController;

    [Header("Pulse Settings")]
    public float scaleAdd;     // how much bigger it gets on each beat
    public float decaySpeed;     // how fast it returns to normal

    [Header("Optional Alpha Pulse")]
    public SpriteRenderer spriteRenderer;
    public float alphaAdd;     // extra alpha on beat
    public float baseAlpha;

    private Vector3 baseScale;
    private float pulseT = 0f;
    private double lastBeatIndex = -999;

    void Awake()
    {
        baseScale = transform.localScale;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (songController == null)
            songController = FindObjectOfType<SongController>();
    }

    void Update()
    {
        if (songController == null) return;

        double songTime = songController.GetSongTime();
        if (songTime < 0) return;

        double spb = songController.GetSecondsPerBeat();
        double beatIndex = System.Math.Floor(songTime / spb);

        //Triggr pulse once per beat
        if (beatIndex != lastBeatIndex)
        {
            lastBeatIndex = beatIndex;
            pulseT = 1f;
        }

        // Decay pulse
        pulseT = Mathf.MoveTowards(pulseT, 0f, Time.deltaTime * decaySpeed);

        // Apply scale pulse
        float s = 1f + (scaleAdd * pulseT);
        transform.localScale = baseScale * s;

        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = Mathf.Clamp01(baseAlpha + (alphaAdd * pulseT));
            spriteRenderer.color = c;
        }
    }
}
