using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongController : MonoBehaviour
{
    public AudioSource audioSource;
    public double songOffset;

    [Header("Song Settings")]
    public float bpm;
    public double songLength;

    [Header("UI")]
    public GameplayUI gameplayUI;
    public float gameplayUIDelay;

    public GameManager gameManager;

    private double dspSongStartTime;
    private double secondsPerBeat;

    private bool started = false;
    private bool songFinished = false;

    void Awake()
    {
        RefreshBeatData();
    }

    void Start()
    {
        RefreshBeatData();
    }

    void Update()
    {
        if (!started || songFinished) return;

        double songTime = GetSongTime();

        if (songTime>= songLength)
        {
            songFinished = true;
            //Debug.Log("SongController: Song finished.");

            if (gameManager != null)
                gameManager.ShowFinalResults();
        }
    }

    public void RefreshBeatData()
    {
        secondsPerBeat = 60f / bpm;
    }

    public void BeginSong()
    {
        if (started) return;

        started = true;
        //Debug.Log("SongController: BeginSong called.");

        RefreshBeatData();

        if (gameplayUI != null) StartCoroutine(ShowGameplayUIDelayed());

        dspSongStartTime = AudioSettings.dspTime + 0.05;
        audioSource.PlayScheduled(dspSongStartTime);

        //Debug.Log("SongController: Audio scheduled.");
    }

    IEnumerator ShowGameplayUIDelayed()
    {
        yield return new WaitForSeconds(gameplayUIDelay);
        gameplayUI.ShowUI();
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