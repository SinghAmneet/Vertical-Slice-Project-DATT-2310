using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text multiText;

    public AudioSource music;
    public bool startPlaying;
    public BeatScroller bs;

    public static GameManager instance;
    public int currentScore;
    public int scorePerNote = 100;

    public int scorePerGoodNote = 125;
    public int scorePerPerfectNote = 150;

    public int currentMultiplier;
    public int multiplierTracker;
    public int[] multiplierThresholds;


    public float totalNotes;
    public float lateHits;
    public float goodHits;
    public float perfectHits;
    public float missedHits;

    public GameObject resultsScreen;
    public TMP_Text percentHitText, lateText, goodText, perfectText, missedText, rankText, finalScoreText;

    void Start()
    {
        instance = this;

        scoreText.text = "Score: "+0;
        currentMultiplier = 1;

        totalNotes = FindObjectsOfType<NoteObject>().Length;

        startPlaying = true;
        bs.hasStarted = true;
        music.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            music.Stop();
        }

        if (!music.isPlaying && !resultsScreen.activeInHierarchy)
        {
            resultsScreen.SetActive(true);
            lateText.text = lateHits.ToString();
            goodText.text = goodHits.ToString();
            perfectText.text = perfectHits.ToString();
            missedText.text = missedHits.ToString();

            float totalHit = lateHits + goodHits + perfectHits;
            float percentage = (totalHit / totalNotes) * 100f;

            percentHitText.text = percentage.ToString("F1") + "%";
        }
    }

    public void NoteHit()
    {
        //Debug.Log("Hit on time");

        if(currentMultiplier-1 < multiplierThresholds.Length)
        {
            multiplierTracker++;

            if (multiplierThresholds[currentMultiplier - 1] <= multiplierTracker)
            {
                multiplierTracker = 0;
                currentMultiplier++;
            }
        }

        multiText.text = "Multiplier: x" + currentMultiplier;

        // currentScore += scorePerNote * currentMultiplier;
        scoreText.text = "Score: " + currentScore;
    }

    public void LateHit()
    {
        currentScore += scorePerNote * currentMultiplier;
        NoteHit();
        lateHits++;
    }

    public void GoodHit()
    {
        currentScore += scorePerGoodNote * currentMultiplier;
        NoteHit();
        goodHits++;
    }

    public void PerfectHit()
    {
        currentScore += scorePerPerfectNote * currentMultiplier;
        NoteHit();
        perfectHits++;
    }

    public void NoteMiss()
    {
        Debug.Log("Missed note");

        currentMultiplier = 1;
        multiplierTracker = 0;

        multiText.text = "Multiplier: x" + currentMultiplier;

        missedHits++;
    }
}
