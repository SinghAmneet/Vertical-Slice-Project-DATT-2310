using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject notePrefab;
    public Transform notesParent;
    public SongController songController;
    public GameManager gameManager;

    [Header("Mode")]
    public bool useManualChart = false;
    public List<NoteData> chart = new List<NoteData>();

    [Header("Timing")]
    public float approachDuration = 0.8f;
    public float previewLead = 0.6f;
    public int spawnBeatStride = 2;
    public double songLength = 63.6;

    [Header("End Behavior")]
    public double endHitCutoffSeconds = 0.5;

    [Header("Start Behavior")]
    public int startingBeatIndex = 1;
    public double missWindow = 0.15;

    [Header("Guide Line")]
    public PathLineManager pathLine;

    [Header("Spawn Spacing")]
    public float minDistanceFromLastNote = 1.2f;
    public float minDistanceFromAnyNote = 0.9f;
    public int maxSpawnAttempts = 25;

    private double secondsPerBeat;
    private int nextBeatIndex;
    private int manualChartIndex = 0;

    private bool firstNoteSpawned = false;
    private NoteObject lastSpawnedNote;

    void Start()
    {
        if (songController == null)
        {
            Debug.LogError("NoteSpawner: songController not assigned.");
            enabled = false;
            return;
        }

        secondsPerBeat = songController.GetSecondsPerBeat();
        chart.Sort((a, b) => a.hitTime.CompareTo(b.hitTime));
        nextBeatIndex = Mathf.Max(1, startingBeatIndex);
    }

    void Update()
    {
        double songTime = songController.GetSongTime();
        if (songTime < 0.0) return;

        if (useManualChart) SpawnFromManualChart(songTime);
        else SpawnAutoBeats(songTime);
    }

    void SpawnFromManualChart(double songTime)
    {
        float spawnLead = previewLead + approachDuration;

        while (manualChartIndex < chart.Count)
        {
            NoteData n = chart[manualChartIndex];

            if (n.hitTime < songTime - missWindow)
            {
                manualChartIndex++;
                continue;
            }

            double spawnTime = n.hitTime - spawnLead;

            if (songTime >= spawnTime && n.hitTime <= songLength)
            {
                Vector2 spawnPos = GetNonOverlappingPosition();

                GameObject obj = Instantiate(notePrefab, spawnPos, Quaternion.identity, notesParent);

                NoteObject noteObj = obj.GetComponent<NoteObject>();
                noteObj.hitTime = n.hitTime;
                noteObj.approachDuration = approachDuration;
                noteObj.previewLead = previewLead;

                if (pathLine != null && lastSpawnedNote != null)
                {
                    pathLine.SetCurrentAndNext(lastSpawnedNote, noteObj);
                }

                lastSpawnedNote = noteObj;

                if (!firstNoteSpawned)
                {
                    noteObj.forceStartActive = true;
                    firstNoteSpawned = true;
                }

                if (gameManager != null)
                    gameManager.RegisterSpawnedNote();

                manualChartIndex++;
            }
            else break;
        }
    }

    void SpawnAutoBeats(double songTime)
    {
        float spawnLead = previewLead + approachDuration;
        double lastAllowedHitTime = songLength - secondsPerBeat - endHitCutoffSeconds;

        while (true)
        {
            double nextBeatTime = nextBeatIndex * secondsPerBeat;

            if (nextBeatTime > lastAllowedHitTime) break;

            if (nextBeatTime < songTime - missWindow)
            {
                nextBeatIndex++;
                continue;
            }

            if (songTime >= (nextBeatTime - spawnLead))
            {
                if (spawnBeatStride <= 0) spawnBeatStride = 1;

                if (nextBeatIndex % spawnBeatStride == 0)
                {
                    SpawnNoteAtRandomPosition(nextBeatTime);
                }

                nextBeatIndex++;
            }
            else break;
        }
    }

    void SpawnNoteAtRandomPosition(double hitTime)
    {
        Vector2 spawnPos = GetNonOverlappingPosition();
        GameObject obj = Instantiate(notePrefab, spawnPos, Quaternion.identity, notesParent);

        NoteObject noteObj = obj.GetComponent<NoteObject>();
        noteObj.hitTime = hitTime;
        noteObj.approachDuration = approachDuration;
        noteObj.previewLead = previewLead;

        if (pathLine != null && lastSpawnedNote != null)
        {
            pathLine.SetCurrentAndNext(lastSpawnedNote, noteObj);
        }

        lastSpawnedNote = noteObj;

        if (!firstNoteSpawned)
        {
            noteObj.forceStartActive = true;
            firstNoteSpawned = true;
        }

        if (gameManager != null)
            gameManager.RegisterSpawnedNote();
    }

    Vector2 GetNonOverlappingPosition()
    {
        Vector2 best = GetRandomScreenPosition();

        for (int attempt = 0; attempt < maxSpawnAttempts; attempt++)
        {
            Vector2 candidate = GetRandomScreenPosition();

            if (lastSpawnedNote != null)
            {
                float dLast = Vector2.Distance(candidate, lastSpawnedNote.transform.position);
                if (dLast < minDistanceFromLastNote)
                    continue;
            }

            bool tooClose = false;
            NoteObject[] notes = FindObjectsOfType<NoteObject>();

            foreach (var n in notes)
            {
                if (n == null) continue;

                float d = Vector2.Distance(candidate, n.transform.position);
                if (d < minDistanceFromAnyNote)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
                continue;

            return candidate;
        }

        return best;
    }

    Vector2 GetRandomScreenPosition()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        float padding = 1f;
        float minX = -width / 2f + padding;
        float maxX = width / 2f - padding;
        float minY = -height / 2f + padding;
        float maxY = height / 2f - padding;

        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }
}