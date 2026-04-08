using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RhythmTutorialManager : MonoBehaviour
{
    [Header("References")]
    public CountdownUI countdownUI;
    public CursorManager cursorManager;

    public GameObject tutorialPanel;
    public GameObject tutorialBackground;
    public TMP_Text tutorialInfoText;
    public TMP_Text tutorialStatusText;

    public TutorialSensitivitySlider sensitivitySlider;

    public GameObject tutorialNotePrefab;
    public Transform tutorialNotesParent;

    [Header("Guide Line")]
    public TutorialStraightLine tutorialPathLine;

    [Header("Tutorial Note Settings")]
    public float noteSpacing;
    public float noteY;
    public float tutorialYOffset;
    public float approachDuration;
    public float hitRadius;
    public float perfectThreshold;
    public float resetDelay;

    [Header("Outro Animation")]
    public float outroDuration;
    public float outroMoveAmount;
    public float outroScaleAmount;

    private readonly List<TutorialNoteObject> tutorialNotes = new List<TutorialNoteObject>();
    private int currentIndex = 0;

    private bool tutorialComplete = false;
    private bool resetting = false;

    private CanvasGroup tutorialPanelCanvasGroup;
    private SpriteRenderer tutorialBackgroundRenderer;

    private Vector3 notesStartPos;
    private Vector3 notesStartScale;

    private Vector3 sliderStartPos;
    private Vector3 sliderStartScale;

    private RectTransform tutorialPanelRect;
    private Vector3 tutorialPanelStartPos;
    private Vector3 tutorialPanelStartScale;

    void Start()
    {
        if (cursorManager != null)
            cursorManager.ResetCursorToCenter();

        if (tutorialInfoText != null)
        {
            tutorialInfoText.text =
                "Tutorial\n" +
                "Click the active note when the ring reaches the center.\n" +
                "Hit all 3 notes PERFECT to begin cooking.";
        }

        if (tutorialStatusText != null)
            tutorialStatusText.text = "";

        if (tutorialPanel != null)
        {
            tutorialPanel.SetActive(true);

            tutorialPanelCanvasGroup = tutorialPanel.GetComponent<CanvasGroup>();
            if (tutorialPanelCanvasGroup == null)
                tutorialPanelCanvasGroup = tutorialPanel.AddComponent<CanvasGroup>();

            tutorialPanelCanvasGroup.alpha = 1f;

            tutorialPanelRect = tutorialPanel.GetComponent<RectTransform>();
            if (tutorialPanelRect != null)
            {
                tutorialPanelStartPos = tutorialPanelRect.localPosition;
                tutorialPanelStartScale = tutorialPanelRect.localScale;
            }
        }

        if (tutorialBackground != null)
        {
            tutorialBackground.SetActive(true);
            tutorialBackgroundRenderer = tutorialBackground.GetComponent<SpriteRenderer>();

            if (tutorialBackgroundRenderer != null)
            {
                Color c = tutorialBackgroundRenderer.color;
                c.a = 1f;
                tutorialBackgroundRenderer.color = c;
            }
        }

        if (tutorialNotesParent != null)
        {
            notesStartPos = tutorialNotesParent.position;
            notesStartScale = tutorialNotesParent.localScale;
        }

        if (sensitivitySlider != null)
        {
            sensitivitySlider.gameObject.SetActive(true);
            sliderStartPos = sensitivitySlider.transform.position;
            sliderStartScale = sensitivitySlider.transform.localScale;
        }

        BuildTutorialSet();
    }

    void Update()
    {
        if (tutorialComplete || resetting)
            return;

        if (currentIndex >= tutorialNotes.Count)
            return;

        TutorialNoteObject activeNote = tutorialNotes[currentIndex];
        if (activeNote == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 cursorPos = cursorManager != null
                ? cursorManager.GetWorldPosition()
                : (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            TutorialJudgeResult result = activeNote.TryJudge(cursorPos, hitRadius, perfectThreshold);

            if (result == TutorialJudgeResult.Perfect)
            {
                Destroy(activeNote.gameObject);
                currentIndex++;

                if (currentIndex >= tutorialNotes.Count)
                {
                    if (tutorialPathLine != null)
                        tutorialPathLine.ClearLine();

                    StartCoroutine(FinishTutorialRoutine());
                }
                else
                {
                    tutorialNotes[currentIndex].BeginActive();

                    // Connect current active note -> next note
                    if (tutorialPathLine != null)
                    {
                        if (currentIndex < tutorialNotes.Count - 1)
                        {
                            TutorialNoteObject current = tutorialNotes[currentIndex];
                            TutorialNoteObject next = tutorialNotes[currentIndex + 1];
                            tutorialPathLine.SetCurrentAndNext(current, next);
                        }
                        else
                        {
                            // No next note left, so clear the line
                            tutorialPathLine.ClearLine();
                        }
                    }

                    if (tutorialStatusText != null)
                        tutorialStatusText.text = "";
                }
            }
        }

        
        if (activeNote != null && activeNote.HasMissed())
        {
            StartCoroutine(ResetTutorialRoutine("Perfect only - try again"));
        }
    }
    void BuildTutorialSet()
    {
        ClearTutorialNotes();
        tutorialNotes.Clear();

        if (tutorialPathLine != null)
            tutorialPathLine.ClearLine();

        currentIndex = 0;

        TutorialNoteObject previousNote = null;

        for (int i = 0; i < 3; i++)
        {
            Vector2 pos = new Vector2((i - 1) * noteSpacing, noteY+tutorialYOffset);

            GameObject obj = Instantiate(tutorialNotePrefab, pos, Quaternion.identity, tutorialNotesParent);
            TutorialNoteObject note = obj.GetComponent<TutorialNoteObject>();

            note.approachDuration = approachDuration;

            if (i == 0)
                note.BeginActive();
            else
                note.SetPreview();

            tutorialNotes.Add(note);

            previousNote = note;
        }

        // Start with line from note 1 -> note 2 only
        if (tutorialPathLine != null && tutorialNotes.Count >= 2)
        {
            tutorialPathLine.SetCurrentAndNext(tutorialNotes[0], tutorialNotes[1]);
        }
    }

    IEnumerator ResetTutorialRoutine(string message)
    {
        if (resetting) yield break;
        resetting = true;

        if (tutorialStatusText != null)
            tutorialStatusText.text = message;

        yield return new WaitForSecondsRealtime(resetDelay);

        BuildTutorialSet();

        if (tutorialStatusText != null)
            tutorialStatusText.text = "";

        resetting = false;
    }

    IEnumerator FinishTutorialRoutine()
    {
        tutorialComplete = true;

        if (tutorialStatusText != null)
            tutorialStatusText.text = "Perfect!";

        yield return new WaitForSecondsRealtime(0.25f);

        if (tutorialPathLine != null)
            tutorialPathLine.ClearLine();

        yield return StartCoroutine(PlayTutorialOutro());

        ClearTutorialNotes();

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        if (tutorialBackground != null)
            tutorialBackground.SetActive(false);

        if (sensitivitySlider != null)
            sensitivitySlider.gameObject.SetActive(false);

        if (countdownUI != null)
            countdownUI.BeginCountdown();
    }

    IEnumerator PlayTutorialOutro()
    {
        float timer = 0f;

        float panelStartAlpha = tutorialPanelCanvasGroup != null ? tutorialPanelCanvasGroup.alpha : 1f;
        float bgStartAlpha = tutorialBackgroundRenderer != null ? tutorialBackgroundRenderer.color.a : 1f;

        Vector3 notesTargetPos = notesStartPos + Vector3.down * outroMoveAmount;
        Vector3 notesTargetScale = notesStartScale * outroScaleAmount;

        Vector3 sliderTargetPos = sliderStartPos + Vector3.down * outroMoveAmount;
        Vector3 sliderTargetScale = sliderStartScale * outroScaleAmount;

        Vector3 panelTargetScale = tutorialPanelStartScale * outroScaleAmount;
        Vector3 panelTargetPos = tutorialPanelStartPos + Vector3.down * (outroMoveAmount * 100f);

        while (timer < outroDuration)
        {
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / outroDuration);

            float easedT = 1f - Mathf.Pow(1f - t, 3f);

            if (tutorialPanelCanvasGroup != null)
                tutorialPanelCanvasGroup.alpha = Mathf.Lerp(panelStartAlpha, 0f, easedT);

            if (tutorialPanelRect != null)
            {
                tutorialPanelRect.localScale = Vector3.Lerp(tutorialPanelStartScale, panelTargetScale, easedT);
                tutorialPanelRect.localPosition = Vector3.Lerp(tutorialPanelStartPos, panelTargetPos, easedT);
            }

            if (tutorialBackgroundRenderer != null)
            {
                Color c = tutorialBackgroundRenderer.color;
                c.a = Mathf.Lerp(bgStartAlpha, 0f, easedT);
                tutorialBackgroundRenderer.color = c;
            }

            if (tutorialNotesParent != null)
            {
                tutorialNotesParent.position = Vector3.Lerp(notesStartPos, notesTargetPos, easedT);
                tutorialNotesParent.localScale = Vector3.Lerp(notesStartScale, notesTargetScale, easedT);
            }

            if (sensitivitySlider != null)
            {
                sensitivitySlider.transform.position = Vector3.Lerp(sliderStartPos, sliderTargetPos, easedT);
                sensitivitySlider.transform.localScale = Vector3.Lerp(sliderStartScale, sliderTargetScale, easedT);
            }

            yield return null;
        }
    }

    void ClearTutorialNotes()
    {
        foreach (Transform child in tutorialNotesParent)
        {
            Destroy(child.gameObject);
        }
    }
}