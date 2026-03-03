using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [Header("References")]
    public GameObject notePrefab;
    public Transform notesParent;
    public SongController songController;

    [Header("Mode")]
    public bool useManualChart = false;
    public List<NoteData> chart = new List<NoteData>();

    [Header("Timing")]
    public float approachDuration = 1.0f;
    public float previewLead = 0.6f;
    public int spawnBeatStride = 2;
    public double songLength = 64.0;    // The length of the song. After 64 sec, the notes stop spawning

    [Header("End Behavior")]
    public double endHitCutoffSeconds = 0.0; 

    [Header("Start Behavior")]
    public int startingBeatIndex = 1;      
    public double missWindow = 0.15;      

    private double secondsPerBeat;
    private int nextBeatIndex;
    private int manualChartIndex = 0;

    private bool firstNoteSpawned = false;

    void Start()
    {
        // 
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

    // IGNORE (Mainly for manual testing)
    // This method is a placeholder if I need to place the notes manually instead of it spawing randomly. 
    // THis is what I started off with.
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
                GameObject obj = Instantiate(notePrefab, n.position, Quaternion.identity, notesParent);

                NoteObject noteObj = obj.GetComponent<NoteObject>();
                noteObj.hitTime = n.hitTime;
                noteObj.approachDuration = approachDuration;
                noteObj.previewLead = previewLead;

                
                if (!firstNoteSpawned)
                {
                    noteObj.forceStartActive = true;
                    firstNoteSpawned = true;
                }

                manualChartIndex++;
            }
            else break;
        }
    }

    // Method is about spawning the notes on beat and random positions.
    void SpawnAutoBeats(double songTime)
    {
        float spawnLead = previewLead + approachDuration;

        // I implemented this line to delete the last note from spawning
        double lastAllowedHitTime = songLength - secondsPerBeat - endHitCutoffSeconds;

        while (true)
        {
            double nextBeatTime = nextBeatIndex * secondsPerBeat;
            // Stop spawning when song time is finished,
            if (nextBeatTime > lastAllowedHitTime) break;

            // If the beat of the song is a bit late, skip it
            if (nextBeatTime < songTime - missWindow)
            {
                nextBeatIndex++;
                continue;
            }

            // Spawn early with a preview of the note with the the ring approaching the hit circle before the actual time
            if (songTime >= (nextBeatTime - spawnLead))
            {
                if (spawnBeatStride <= 0) spawnBeatStride = 1;

                if (nextBeatIndex % spawnBeatStride == 0)
                {
                    SpawnNoteAtRandomPosition(nextBeatTime);
                }

                nextBeatIndex++;
            }
            else break; // added this when not time to spawn the next note yet.
        }
    }

    // spawn note randomly on screen (for SpawnAutoBeats() method.)
    void SpawnNoteAtRandomPosition(double hitTime)
    {
        Vector2 spawnPos = GetRandomScreenPosition();
        GameObject obj = Instantiate(notePrefab, spawnPos, Quaternion.identity, notesParent);

        NoteObject noteObj = obj.GetComponent<NoteObject>();
        noteObj.hitTime = hitTime;
        noteObj.approachDuration = approachDuration;
        noteObj.previewLead = previewLead;

        
        if (!firstNoteSpawned)
        {
            noteObj.forceStartActive = true;
            firstNoteSpawned = true;
        }
    }

    // This method picks a random position inside the ortho camera view.
    // Padding is for the notes to not spawn on the edges of the camera.
    Vector2 GetRandomScreenPosition()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        float padding = 0.6f;
        float minX = -width / 2f + padding;
        float maxX = width / 2f - padding;
        float minY = -height / 2f + padding;
        float maxY = height / 2f - padding;

        return new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }
}