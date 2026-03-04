using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongController : MonoBehaviour
{
    public AudioSource audioSource;
    public double songOffset = 0.0;

    [Header("Song Settings")]
    public float bpm = 111f;
    public double songLength = 63.6; // length of your song in seconds

    private double dspSongStartTime;
    private double secondsPerBeat;

    private bool started = false;
    private bool songFinished = false; // prevents multiple console prints

    void Awake()
    {
        secondsPerBeat = 60f / bpm;
    }

    void Update()
    {
        if (!started || songFinished) return;

        double songTime = GetSongTime();

        // Detect when the song finishes
        if (songTime >= songLength)
        {
            songFinished = true;
            Debug.Log("The song has finished playing!");
        }
    }

    // Called by CountdownUI when COOK! appears
    public void BeginSong()
    {
        if (started) return;

        started = true;

        // Start the audio using DSP time for accurate syncing
        dspSongStartTime = AudioSettings.dspTime + 0.05;
        audioSource.PlayScheduled(dspSongStartTime);
    }

    public double GetSongTime()
    {
        if (!started) return -999.0;

        return AudioSettings.dspTime - dspSongStartTime - songOffset;
    }

    public double GetSecondsPerBeat()
    {
        return secondsPerBeat;
    }
}