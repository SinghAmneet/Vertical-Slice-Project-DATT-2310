using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongController : MonoBehaviour
{
    public AudioSource audioSource;
    public double songOffset = 0.0;

    [Header("Song Settings")]
    public float bpm = 111f;
    public double songLength = 63.6;

    [Header("UI")]
    public GameplayUI gameplayUI;
    public float gameplayUIDelay = 0.3f; // small delay after COOK!

    public GameManager gameManager;

    private double dspSongStartTime;
    private double secondsPerBeat;

    private bool started = false;
    private bool songFinished = false;

    void Awake()
    {
        secondsPerBeat = 60f / bpm;
    }

    void Update()
    {
        if (!started || songFinished) return;

        double songTime = GetSongTime();

        if (songTime >= songLength)
        {
            songFinished = true;
            Debug.Log("The song has finished playing!");

            if (gameManager != null)
                gameManager.ShowFinalResults();
        }
    }

    public void BeginSong()
    {
        if (started) return;

        started = true;

        // Show gameplay UI shortly after COOK!
        if (gameplayUI != null)
            StartCoroutine(ShowGameplayUIDelayed());

        dspSongStartTime = AudioSettings.dspTime + 0.05;
        audioSource.PlayScheduled(dspSongStartTime);
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