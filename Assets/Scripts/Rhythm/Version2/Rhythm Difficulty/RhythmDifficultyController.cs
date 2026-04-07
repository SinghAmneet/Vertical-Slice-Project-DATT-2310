using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmDifficultyController : MonoBehaviour
{
    [Header("References")]
    public SongController songController;
    public NoteSpawner noteSpawner;

    [Header("Audio Clips")]
    public AudioClip easySong;
    public AudioClip mediumSong;
    public AudioClip hardSong;

    void Awake()
    {
        ApplyDifficultySettings();
    }

    void ApplyDifficultySettings()
    {
        if (songController == null)
        {
            Debug.LogError("RhythmDifficultyController: SongController is missing.");
            return;
        }

        if (songController.audioSource == null)
        {
            Debug.LogError("RhythmDifficultyController: SongController audioSource is missing.");
            return;
        }

        RhythmDifficulty currentDifficulty = RhythmProgressData.GetCurrentDifficulty();

        switch (currentDifficulty)
        {
            case RhythmDifficulty.Easy:
                songController.audioSource.clip = easySong;
                songController.bpm = 89f;
                songController.songLength = 28.0;

                if (noteSpawner != null)
                {
                    noteSpawner.songLength = 28.0;
                    noteSpawner.spawnBeatStride = 2;
                    noteSpawner.approachDuration = 1.0f;
                }
                break;

            case RhythmDifficulty.Medium:
                songController.audioSource.clip = mediumSong;
                songController.bpm = 111f;
                songController.songLength = 22.0;

                if (noteSpawner != null)
                {
                    noteSpawner.songLength = 22.0;
                    noteSpawner.spawnBeatStride = 2;
                    noteSpawner.approachDuration = 0.8f;
                }
                break;

            case RhythmDifficulty.Hard:
                songController.audioSource.clip = hardSong;
                songController.bpm = 111f;
                songController.songLength = 63.6;

                if (noteSpawner != null)
                {
                    noteSpawner.songLength = 63.6;
                    noteSpawner.spawnBeatStride = 2;
                    noteSpawner.approachDuration = 0.8f;
                }
                break;
        }

        songController.RefreshBeatData();

        Debug.Log("Rhythm difficulty set to: " + currentDifficulty);
    }
}