using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialJudgeResult
{
    None,
    Perfect,
    Fail
}

public class TutorialNoteObject : MonoBehaviour
{
    [Header("Timing")]
    public float approachDuration = 0.8f;
    public float missWindow = 0.15f;

    [Header("References")]
    public Transform hitCircle;
    public Transform approachCircle;
    public SpriteRenderer hitRenderer;
    public SpriteRenderer approachRenderer;

    private enum VisualState { Preview, Active, Locked }
    private VisualState state = VisualState.Preview;

    private float activeTimer = 0f;
    private bool missed = false;

    void Awake()
    {
        if (hitRenderer == null && hitCircle != null)
            hitRenderer = hitCircle.GetComponent<SpriteRenderer>();

        if (approachRenderer == null && approachCircle != null)
            approachRenderer = approachCircle.GetComponent<SpriteRenderer>();

        ApplyPreviewVisuals();
    }

    void Update()
    {
        if (state != VisualState.Active)
            return;

        activeTimer += Time.unscaledDeltaTime;

        float progress = Mathf.Clamp01(activeTimer / approachDuration);
        float easedProgress = progress * progress;
        float scale = Mathf.Lerp(1.6f, 0.5f, easedProgress);

        if (approachCircle != null)
            approachCircle.localScale = Vector3.one * scale;

        if (activeTimer > approachDuration + missWindow)
            missed = true;
    }

    public void BeginActive()
    {
        state = VisualState.Active;
        activeTimer = 0f;
        missed = false;
        ApplyActiveVisuals();
    }

    public void SetPreview()
    {
        state = VisualState.Preview;
        ApplyPreviewVisuals();
    }

    public TutorialJudgeResult TryJudge(Vector2 cursorWorldPos, float hitRadius, float perfectThreshold)
    {
        if (state != VisualState.Active)
            return TutorialJudgeResult.None;

        float distance = Vector2.Distance(cursorWorldPos, transform.position);
        if (distance > hitRadius)
            return TutorialJudgeResult.None;

        float progress = Mathf.Clamp01(activeTimer / approachDuration);

        if (progress >= perfectThreshold)
        {
            state = VisualState.Locked;
            return TutorialJudgeResult.Perfect;
        }

        return TutorialJudgeResult.Fail;
    }

    public bool HasMissed()
    {
        return missed;
    }

    public bool IsActiveState()
    {
        return state == VisualState.Active;
    }

    void ApplyPreviewVisuals()
    {
        if (hitRenderer != null)
        {
            Color c = hitRenderer.color;
            c.a = 1f;
            hitRenderer.color = c;
        }

        if (approachRenderer != null)
        {
            Color c = approachRenderer.color;
            c.a = 0f;
            approachRenderer.color = c;
        }

        if (approachCircle != null)
            approachCircle.localScale = Vector3.one * 1.6f;
    }

    void ApplyActiveVisuals()
    {
        if (hitRenderer != null)
        {
            Color c = hitRenderer.color;
            c.a = 1f;
            hitRenderer.color = c;
        }

        if (approachRenderer != null)
        {
            Color c = approachRenderer.color;
            c.a = 1f;
            approachRenderer.color = c;
        }
    }
}
