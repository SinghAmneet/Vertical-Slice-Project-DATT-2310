using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public SongController songController;
    public GameManager gameManager;
    public float hitRadius = 0.5f;

    [Header("Judgement by Approach Progress (0 to 1)")]
    [Range(0f, 1f)] public float perfectThreshold = 0.90f;
    [Range(0f, 1f)] public float goodThreshold = 0.70f;

    [Header("Strict Hit Lock")]
    [Range(0f, 1f)] public float minClickableProgress = 0.25f;

    [Header("Safety")]
    public double missWindow = 0.15;

    [Header("Popup Prefabs")]
    public GameObject perfectPopupPrefab;
    public GameObject goodPopupPrefab;
    public GameObject latePopupPrefab;
    public GameObject missPopupPrefab;
    public GameObject firePopupPrefab;
    public GameObject oneXPopupPrefab; // NEW: plays when multiplier drops to 1x after a miss

    public Vector3 popupOffset = Vector3.zero;

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

        // Too early
        if (timeUntilHit > note.approachDuration)
            return;

        // Too late
        if (timeUntilHit < -missWindow)
            return;

        float progress = 1f - (float)(timeUntilHit / note.approachDuration);
        progress = Mathf.Clamp01(progress);

        if (progress < minClickableProgress)
            return;

        string judgement;

        if (progress >= perfectThreshold)
        {
            judgement = "PERFECT";
            SpawnPopup(perfectPopupPrefab, note.transform.position);
        }
        else if (progress >= goodThreshold)
        {
            judgement = "GOOD";
            SpawnPopup(goodPopupPrefab, note.transform.position);
        }
        else
        {
            judgement = "LATE";
            SpawnPopup(latePopupPrefab, note.transform.position);
        }

        Debug.Log(judgement);

        bool multiplierIncreased = false;
        if (gameManager != null)
            multiplierIncreased = gameManager.RegisterJudgement(judgement);

        // Fire effect when multiplier levels up
        if (multiplierIncreased)
            SpawnPopup(firePopupPrefab, note.transform.position);

        double signedOffset = songTime - note.hitTime;
        note.Judge(signedOffset);
    }

    public void RegisterMissAtPosition(Vector3 worldPosition)
    {
        SpawnPopup(missPopupPrefab, worldPosition);
        Debug.Log("MISS");

        bool droppedTo1X = false;
        if (gameManager != null)
            droppedTo1X = gameManager.RegisterMissAndCheckMultiplierDrop();

        // If multiplier was 2x or higher before the miss, show the 1x popup
        if (droppedTo1X)
            SpawnPopup(oneXPopupPrefab, worldPosition);
    }

    public void RegisterMiss()
    {
        Debug.Log("MISS");

        bool droppedTo1X = false;
        if (gameManager != null)
            droppedTo1X = gameManager.RegisterMissAndCheckMultiplierDrop();

        if (droppedTo1X)
            SpawnPopup(oneXPopupPrefab, Vector3.zero);
    }

    void SpawnPopup(GameObject popupPrefab, Vector3 worldPosition)
    {
        if (popupPrefab == null) return;

        Instantiate(popupPrefab, worldPosition + popupOffset, Quaternion.identity);
    }
}