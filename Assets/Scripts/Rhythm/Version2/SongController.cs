using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongController : MonoBehaviour
{
    public AudioSource audioSource;
    public double songOffset = 0.0; 
    
    public float bpm = 111f; // Current song bpm
    private double dspSongStartTime;
    private double secondsPerBeat;

    void Start()
    {
        secondsPerBeat = 60f / bpm;
        dspSongStartTime = AudioSettings.dspTime + 1.0; // 1 sec delay
        audioSource.PlayScheduled(dspSongStartTime);
    }

    public double GetSongTime()
    {
        return AudioSettings.dspTime - dspSongStartTime - songOffset;
    }

    public double GetSecondsPerBeat()
    {
        return secondsPerBeat;
    }
}
