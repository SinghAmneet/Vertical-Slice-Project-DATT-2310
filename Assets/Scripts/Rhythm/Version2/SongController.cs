using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongController : MonoBehaviour
{
    public AudioSource audioSource;
    public double songOffset = 0.0; // Just in case if the music doesn't start at 0
    
    public float bpm = 111f; // Current song bpm
    private double dspSongStartTime;    // FOr stable timing which will help to not drift during frame rate.
    private double secondsPerBeat;

    void Start()
    {
        secondsPerBeat = 60f / bpm; // Tempo calc
        dspSongStartTime = AudioSettings.dspTime + 1.0; // 1 sec delay at the start of the song
        audioSource.PlayScheduled(dspSongStartTime);
    }

    // Method for returning the current song time in sec when the track started.
    public double GetSongTime()
    {
        return AudioSettings.dspTime - dspSongStartTime - songOffset;
    }

    // FOr NoteSpawner.cs to place the notes on beat
    public double GetSecondsPerBeat()
    {
        return secondsPerBeat;
    }
}
