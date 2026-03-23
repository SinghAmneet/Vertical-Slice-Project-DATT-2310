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
    public TMP_Text tutorialInfoText;
    public TMP_Text tutorialStatusText;

    public TutorialSensitivitySlider sensitivitySlider;

    public GameObject tutorialNotePrefab;
    public Transform tutorialNotesParent;

    [Header("Tutorial Note Settings")]
    public float noteSpacing = 3f;
    public float noteY = -0.5f;
    public float approachDuration = 0.8f;
    public float hitRadius = 0.5f;
    public float perfectThreshold = 0.90f;
    public float resetDelay = 0.5f;

    private readonly List<TutorialNoteObject> tutorialNotes = new List<TutorialNoteObject>();
    private int currentIndex = 0;

    private bool tutorialComplete = false;
    private bool resetting = false;

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
            tutorialPanel.SetActive(true);

        if (sensitivitySlider != null)
            sensitivitySlider.gameObject.SetActive(true);

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
                    StartCoroutine(FinishTutorialRoutine());
                }
                else
                {
                    tutorialNotes[currentIndex].BeginActive();

                    if (tutorialStatusText != null)
                        tutorialStatusText.text = "";
                }
            }
            else if (result == TutorialJudgeResult.Fail)
            {
                StartCoroutine(ResetTutorialRoutine("Perfect only - try again"));
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

        currentIndex = 0;

        for (int i = 0; i < 3; i++)
        {
            Vector2 pos = new Vector2((i - 1) * noteSpacing, noteY);

            GameObject obj = Instantiate(tutorialNotePrefab, pos, Quaternion.identity, tutorialNotesParent);
            TutorialNoteObject note = obj.GetComponent<TutorialNoteObject>();

            note.approachDuration = approachDuration;

            if (i == 0) note.BeginActive();
            else note.SetPreview();

            tutorialNotes.Add(note);
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

        ClearTutorialNotes();

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        if (sensitivitySlider != null)
            sensitivitySlider.gameObject.SetActive(false);

        if (countdownUI != null)
            countdownUI.BeginCountdown();
    }

    void ClearTutorialNotes()
    {
        foreach (Transform child in tutorialNotesParent)
        {
            Destroy(child.gameObject);
        }
    }
}