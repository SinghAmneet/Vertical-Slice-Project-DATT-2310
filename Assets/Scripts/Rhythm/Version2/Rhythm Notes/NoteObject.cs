using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    [Header("Timing")]
    public double hitTime;
    public float approachDuration;
    public float previewLead;

    [Header("Preview Rules")]
    public float noPreviewForFirstSeconds;
    public bool forceStartActive = false;

    [Header("References")]
    public Transform hitCircle;
    public Transform approachCircle;
    public SpriteRenderer hitRenderer;
    public SpriteRenderer approachRenderer;

    [Header("Preview Look")]
    [Range(0f, 1f)] public float previewAlpha;
    public Color previewTint = new Color(0.8f, 0.8f, 0.8f, 1f);

    private SongController songController;
    private InputManager inputManager;
    private bool judged = false;

    private enum VisualState { Preview, Active }
    private VisualState state;

    void Start()
    {
        songController = FindObjectOfType<SongController>();
        inputManager = FindObjectOfType<InputManager>();

        if (hitRenderer == null && hitCircle != null)
            hitRenderer = hitCircle.GetComponent<SpriteRenderer>();

        if (approachRenderer == null && approachCircle != null)
            approachRenderer = approachCircle.GetComponent<SpriteRenderer>();

        if (forceStartActive || hitTime <= noPreviewForFirstSeconds)
            SetState(VisualState.Active);
        else
            SetState(VisualState.Preview);
    }

    void Update()
    {
        if (judged || songController == null) return;

        double songTime = songController.GetSongTime();
        double timeUntilHit = hitTime - songTime;

        if (state == VisualState.Preview)
        {
            ApplyPreviewVisuals();

            if (timeUntilHit <= approachDuration)
                SetState(VisualState.Active);
            else
                return;
        }

        ApplyActiveVisuals();

        float progress = 1f - (float)(timeUntilHit / approachDuration);
        progress = Mathf.Clamp01(progress);

        float easedProgress = progress * progress;
        float scale = Mathf.Lerp(1.6f, 0.5f, easedProgress);

        if (approachCircle != null)
            approachCircle.localScale = Vector3.one * scale;

        if (timeUntilHit < -0.15f)
        {
            AutoMiss();
        }
    }

    void AutoMiss()
    {
        if (judged) return;

        judged = true;

        if (inputManager != null)
            inputManager.RegisterMissAtPosition(transform.position);

        Destroy(gameObject);
    }

    void SetState(VisualState newState)
    {
        state = newState;

        if (state == VisualState.Preview) ApplyPreviewVisuals();
        else ApplyActiveVisuals();
    }

    void ApplyPreviewVisuals()
    {
        if (hitRenderer != null)
        {
            Color c = previewTint;
            c.a = previewAlpha;
            hitRenderer.color = c;
        }

        if (approachRenderer != null)
        {
            Color c = approachRenderer.color;
            c.a = 0.0f;
            approachRenderer.color = c;
        }

        if (approachCircle != null)
            approachCircle.localScale = Vector3.one * 1.6f;
    }

    void ApplyActiveVisuals()
    {
        if (hitRenderer != null)
        {
            hitRenderer.color = Color.white;
        }

        if (approachRenderer != null)
        {
            Color c = Color.white;
            c.a = 1.0f;
            approachRenderer.color = c;
        }
    }

    public void Judge(double offset)
    {
        if (judged) return;

        judged = true;
        Destroy(gameObject);
    }

    public bool IsActiveState()
    {
        return state == VisualState.Active;
    }
}