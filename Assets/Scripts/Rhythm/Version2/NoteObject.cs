using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    [Header("Timing")]
    public double hitTime;
    public float approachDuration = 1.0f;   // ring collapse is sec
    public float previewLead = 0.6f;    // preview timing

    [Header("Preview Rules")]
    public float noPreviewForFirstSeconds = 1.0f;
    public bool forceStartActive = false;

    [Header("References")]
    public Transform hitCircle;
    public Transform approachCircle;
    // For hiding preview
    public SpriteRenderer hitRenderer;
    public SpriteRenderer approachRenderer;

    private SongController songController;
    private InputManager inputManager;
    private bool judged = false;    // For note processing

    private enum VisualState { Preview, Active }    // visual state: Preview is the foreshadow and Active is the animated ring
    private VisualState state;

    void Start()
    {
        songController = FindObjectOfType<SongController>();
        inputManager = FindObjectOfType<InputManager>();

        // Data Valaidation if render in prefab is undefined.
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
            ApplyPreviewVisuals(); // Provides the preview to hide the ring every frame

            // When insdie the approach window, flip to active note
            if (timeUntilHit <= approachDuration)
                SetState(VisualState.Active);
            else
                return;
        }

        ApplyActiveVisuals(); // Active note visual

        float progress = 1f - (float)(timeUntilHit / approachDuration);
        progress = Mathf.Clamp01(progress);

        float easedProgress = progress * progress;  // GIves a nice easing effect making the ring get faster towards the hit circle
        float scale = Mathf.Lerp(1.6f, 0.5f, easedProgress);    // Animation ring goiing big to small towards the hitting circle

        if (approachCircle != null)
            approachCircle.localScale = Vector3.one * scale;

        // If the player doesn't hit note after hit timing, treat as a missed note.
        if (timeUntilHit < -0.15f)
        {
            AutoMiss();
        }
    }

    void AutoMiss()
    {
        if (judged) return;

        judged = true;

        if (inputManager != null) inputManager.RegisterMissAtPosition(transform.position);

        Destroy(gameObject);
    }

    // Switching between visuals fro active and preview
    void SetState(VisualState newState)
    {
        state = newState;

        if (state == VisualState.Preview) ApplyPreviewVisuals();
        else ApplyActiveVisuals();
    }

    //preview
    void ApplyPreviewVisuals()
    {
        if (hitRenderer != null)
        {
            Color c = hitRenderer.color;
            c.a = 1.0f;
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

    // active
    void ApplyActiveVisuals()
    {
        if (hitRenderer != null)
        {
            Color c = hitRenderer.color;
            c.a = 1.0f;
            hitRenderer.color = c;
        }

        if (approachRenderer != null)
        {
            Color c = approachRenderer.color;
            c.a = 1.0f;
            approachRenderer.color = c;
        }
    }

    // Called by InputManager.cs when player clicks on the note for processing.
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