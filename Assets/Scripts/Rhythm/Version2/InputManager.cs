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
        {
            JudgeByApproachProgress(bestNote, songTime);
        }
    }

    void JudgeByApproachProgress(NoteObject note, double songTime)
    {
        double timeUntilHit = note.hitTime - songTime;

        
        if (timeUntilHit < -missWindow)
            return;

        float progress;
        if (timeUntilHit > note.approachDuration)
        {
            progress = 0f;
        }
        else
        {
            progress = 1f - (float)(timeUntilHit / note.approachDuration);
            progress = Mathf.Clamp01(progress);
        }

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