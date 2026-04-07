using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public SpriteRenderer cursorRenderer;

    [Header("Cursor Sprites")]
    public Sprite defaultCursorSprite;
    public Sprite hoveringNoteCursorSprite;
    public Sprite clickingNoteCursorSprite;

    [Header("Cursor Movement")]
    [Tooltip("Base cursor speed. Increase if it feels too slow.")]
    public float baseSpeed = 25f;

    [Tooltip("Adjusted by the tutorial slider.")]
    public float sensitivity = 1.0f;

    public float hoverRadius = 0.7f;
    public float clickSpriteDuration = 0.08f;

    private Vector2 virtualScreenPosition;
    private float clickTimer = 0f;
    private bool gameplayCursorActive = true;
    private bool forceHover = false;

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (cursorRenderer == null)
            cursorRenderer = GetComponent<SpriteRenderer>();

        ResetCursorToCenter();
        SetGameplayCursorActive(true);
    }

    void Update()
    {
        if (!gameplayCursorActive || mainCamera == null)
            return;

        // --- TRUE virtual cursor movement ---
        Vector2 delta = new Vector2(
            Input.GetAxisRaw("Mouse X"),
            Input.GetAxisRaw("Mouse Y")
        );

        float moveX = delta.x * baseSpeed * sensitivity;
        float moveY = delta.y * baseSpeed * sensitivity;

        virtualScreenPosition += new Vector2(moveX, moveY);

        // Clamp inside screen
        virtualScreenPosition.x = Mathf.Clamp(virtualScreenPosition.x, 0f, Screen.width);
        virtualScreenPosition.y = Mathf.Clamp(virtualScreenPosition.y, 0f, Screen.height);

        UpdateWorldPositionFromVirtual();

        // Click animation
        if (Input.GetMouseButtonDown(0))
            clickTimer = clickSpriteDuration;

        if (clickTimer > 0f)
            clickTimer -= Time.unscaledDeltaTime;

        UpdateCursorSprite();
    }

    void UpdateWorldPositionFromVirtual()
    {
        if (mainCamera == null) return;

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(
            new Vector3(
                virtualScreenPosition.x,
                virtualScreenPosition.y,
                Mathf.Abs(mainCamera.transform.position.z)
            )
        );

        worldPos.z = 0f;
        transform.position = worldPos;
    }

    void UpdateCursorSprite()
    {
        if (cursorRenderer == null)
            return;

        // Click has highest priority
        if (clickTimer > 0f && clickingNoteCursorSprite != null)
        {
            cursorRenderer.sprite = clickingNoteCursorSprite;
            return;
        }

        // Force hover while dragging slider
        if (forceHover && hoveringNoteCursorSprite != null)
        {
            cursorRenderer.sprite = hoveringNoteCursorSprite;
            return;
        }

        // Normal hover over notes
        if (IsHoveringActiveNote() && hoveringNoteCursorSprite != null)
        {
            cursorRenderer.sprite = hoveringNoteCursorSprite;
            return;
        }

        // Default sprite
        if (defaultCursorSprite != null)
            cursorRenderer.sprite = defaultCursorSprite;
    }

    bool IsHoveringActiveNote()
    {
        Vector2 cursorPos = GetWorldPosition();

        // Rhythm notes
        NoteObject[] rhythmNotes = FindObjectsOfType<NoteObject>();
        foreach (var note in rhythmNotes)
        {
            if (note == null || !note.IsActiveState()) continue;

            float d = Vector2.Distance(cursorPos, note.transform.position);
            if (d <= hoverRadius) return true;
        }

        // Tutorial notes
        TutorialNoteObject[] tutorialNotes = FindObjectsOfType<TutorialNoteObject>();
        foreach (var note in tutorialNotes)
        {
            if (note == null || !note.IsActiveState()) continue;

            float d = Vector2.Distance(cursorPos, note.transform.position);
            if (d <= hoverRadius) return true;
        }

        return false;
    }

    public Vector2 GetWorldPosition()
    {
        return transform.position;
    }

    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = Mathf.Clamp(newSensitivity, 0.25f, 3f);
    }

    public float GetSensitivity()
    {
        return sensitivity;
    }

    public void SetGameplayCursorActive(bool active)
    {
        gameplayCursorActive = active;

        if (cursorRenderer != null)
            cursorRenderer.enabled = active;

        if (active)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            ResetCursorToCenter();
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ResetCursorToCenter()
    {
        virtualScreenPosition = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        UpdateWorldPositionFromVirtual();
    }

    public void SetForceHover(bool state)
    {
        forceHover = state;
    }
}