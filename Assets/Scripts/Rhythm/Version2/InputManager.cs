using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public SongController songController;
    public float hitRadius = 0.5f;

    [Header("Judgement by Approach Progress (0 to 1)")]
    [Tooltip("If approach progress is >= this, it's PERFECT (very close).")]
    [Range(0f, 1f)] public float perfectThreshold = 0.90f;

    [Tooltip("If approach progress is >= this (but < perfect), it's GOOD (about halfway+).")]
    [Range(0f, 1f)] public float goodThreshold = 0.70f;

    [Header("Strict Hit Lock")]
    [Tooltip("Player cannot click until approach progress reaches this value. (0.0 = not strict, 0.3+ = strict)")]
    [Range(0f, 1f)] public float minClickableProgress = 0.25f;

    [Header("Safety")]
    [Tooltip("Ignore clicks for notes that are already past this late window (seconds).")]
    public double missWindow = 0.15;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryHit();
    }

    void TryHit()
    {
        double songTime = songController.GetSongTime();
        NoteObject[] notes = FindObjectsOfType<NoteObject>();

        NoteObject bestNote = null;
        double bestDistance = double.MaxValue;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (var note in notes)
        {
            float distance = Vector2.Distance(mousePos, note.transform.position);
            if (distance > hitRadius) continue;

            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestNote = note;
            }
        }

        if (bestNote != null)
            JudgeByApproachProgress(bestNote, songTime);
    }

    void JudgeByApproachProgress(NoteObject note, double songTime)
    {
        double timeUntilHit = note.hitTime - songTime;

        // Too late: ignore (note will auto-miss)
        if (timeUntilHit < -missWindow)
            return;

        // Too early: outside approach window entirely
        if (timeUntilHit > note.approachDuration)
            return;

        // Compute approach progress (0..1)
        float progress = 1f - (float)(timeUntilHit / note.approachDuration);
        progress = Mathf.Clamp01(progress);

        // STRICT: don't allow hits until the ring has meaningfully started collapsing
        if (progress < minClickableProgress)
            return;

        if (progress >= perfectThreshold)
            Debug.Log("PERFECT");
        else if (progress >= goodThreshold)
            Debug.Log("GOOD");
        else
            Debug.Log("LATE");

        double signedOffset = songTime - note.hitTime;
        note.Judge(signedOffset);
    }

    public void RegisterMiss()
    {
        Debug.Log("MISS");
    }
}