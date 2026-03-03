using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public SongController songController;

    public float hitRadius = 0.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryHit();
        }
    }

    void TryHit()
    {
        double songTime = songController.GetSongTime();

        NoteObject[] notes = FindObjectsOfType<NoteObject>();

        NoteObject bestNote = null;
        double bestOffset = double.MaxValue;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        foreach (var note in notes)
        {
            double offset = songTime - note.hitTime;
            double abs = System.Math.Abs(offset);

            float distance = Vector2.Distance(mousePos, note.transform.position);

            if (distance <= hitRadius && abs < bestOffset)
            {
                bestOffset = abs;
                bestNote = note;
            }
        }

        if (bestNote != null)
        {
            Judge(bestNote, bestOffset);
        }
    }

    void Judge(NoteObject note, double absOffset)
    {
        if (absOffset <= 0.1)
            Debug.Log("PERFECT");
        else if (absOffset <= 0.3)
            Debug.Log("GREAT");
        else if (absOffset <= 0.5)
            Debug.Log("GOOD");
        else
            Debug.Log("MISS");

        note.Judge(absOffset);
    }
}
