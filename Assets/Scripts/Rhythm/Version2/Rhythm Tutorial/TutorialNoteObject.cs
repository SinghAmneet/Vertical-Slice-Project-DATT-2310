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

    [Header("Sound Effects")]
    public AudioSource sfxSource;
    public AudioClip noteClickSound;

    [Header("References")]
    public Transform hitCircle;
    public Transform approachCircle;
    public SpriteRenderer hitRenderer;
    public SpriteRenderer approachRenderer;

    [Header("Tutorial Popups")]
    public GameObject perfectPopupPrefab;
    public GameObject failPopupPrefab;
    public Vector3 popupOffset = Vector3.zero;

    [Header("Preview Look")]
    [Range(0f, 1f)] public float previewAlpha = 0.45f;
    public Color previewTint = new Color(0.8f, 0.8f, 0.8f, 1f);

    private enum VisualState { Preview, Active, Locked }
    private VisualState state = VisualState.Preview;

    private float activeTimer = 0f;
    private bool missed = false;
    private bool failPopupShown = false;

    void Awake()
    {
        if (hitRenderer == null && hitCircle != null)
            hitRenderer = hitCircle.GetComponent<SpriteRenderer>();

        if (approachRenderer == null && approachCircle != null)
            approachRenderer = approachCircle.GetComponent<SpriteRenderer>();

        // ALWAYS prefer the shared scene SFX source from InputManager
        InputManager inputManager = FindObjectOfType<InputManager>();
        if (inputManager != null && inputManager.sfxSource != null)
            sfxSource = inputManager.sfxSource;

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
        {
            missed = true;

            if (!failPopupShown)
            {
                ShowFailPopup();
                failPopupShown = true;
            }
        }
    }

    public void BeginActive()
    {
        state = VisualState.Active;
        activeTimer = 0f;
        missed = false;
        failPopupShown = false;
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
            ShowPerfectPopup();
            PlayNoteClickSound();
            return TutorialJudgeResult.Perfect;
        }

        if (!failPopupShown)
        {
            ShowFailPopup();
            failPopupShown = true;
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
            Color c = previewTint;
            c.a = previewAlpha;
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
            hitRenderer.color = Color.white;

        if (approachRenderer != null)
        {
            Color c = Color.white;
            c.a = 1f;
            approachRenderer.color = c;
        }
    }

    void PlayNoteClickSound()
    {
        if (sfxSource == null)
        {
            Debug.LogWarning("TutorialNoteObject: sfxSource is missing.");
            return;
        }

        if (noteClickSound == null)
        {
            Debug.LogWarning("TutorialNoteObject: noteClickSound is missing.");
            return;
        }

        sfxSource.pitch = Random.Range(0.97f, 1.03f);
        sfxSource.PlayOneShot(noteClickSound);
    }

    void ShowPerfectPopup()
    {
        if (perfectPopupPrefab != null)
            Instantiate(perfectPopupPrefab, transform.position + popupOffset, Quaternion.identity);
    }

    void ShowFailPopup()
    {
        if (failPopupPrefab != null)
            Instantiate(failPopupPrefab, transform.position + popupOffset, Quaternion.identity);
    }
}