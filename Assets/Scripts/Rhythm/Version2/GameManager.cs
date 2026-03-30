using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Score Values")]
    public int perfectPoints = 300;
    public int goodPoints = 150;
    public int earlyPoints = 50;

    [Header("Multiplier Settings")]
    public int comboPerMultiplierStep = 5;
    public int maxMultiplier = 8;

    [Header("UI")]
    public ResultsUI resultsUI;
    public GameplayUI gameplayUI;

    private int score = 0;
    private int combo = 0;
    private int multiplier = 1;

    private int perfectCount = 0;
    private int goodCount = 0;
    private int earlyCount = 0;
    private int missCount = 0;

    private int totalNotes = 0;

    private int rawJudgementScore = 0;
    private int maxRawJudgementScore = 0;

    private bool resultsShown = false;

    void Start()
    {
        UpdateGameplayUI();
    }

    public void RegisterSpawnedNote()
    {
        totalNotes++;
        maxRawJudgementScore += perfectPoints;
        UpdateGameplayUI();
    }

    public bool RegisterJudgement(string judgement)
    {
        int previousMultiplier = multiplier;

        switch (judgement)
        {
            case "PERFECT":
                perfectCount++;
                combo++;
                UpdateMultiplier();
                score += perfectPoints * multiplier;
                rawJudgementScore += perfectPoints;
                break;

            case "GOOD":
                goodCount++;
                combo++;
                UpdateMultiplier();
                score += goodPoints * multiplier;
                rawJudgementScore += goodPoints;
                break;

            case "EARLY":
                earlyCount++;
                combo++;
                UpdateMultiplier();
                score += earlyPoints * multiplier;
                rawJudgementScore += earlyPoints;
                break;

            case "MISS":
                missCount++;
                combo = 0;
                multiplier = 1;
                break;
        }

        UpdateGameplayUI();
        return multiplier > previousMultiplier;
    }

    public bool RegisterMissAndCheckMultiplierDrop()
    {
        int previousMultiplier = multiplier;

        missCount++;

        if (ScreenShake.Instance != null)
        {
            if (previousMultiplier >= 4)
                ScreenShake.Instance.Shake(0.14f, 0.07f);
            else
                ScreenShake.Instance.Shake();
        }

        combo = 0;
        multiplier = 1;

        UpdateGameplayUI();

        return previousMultiplier >= 2;
    }

    void UpdateMultiplier()
    {
        multiplier = 1 + (combo / comboPerMultiplierStep);
        multiplier = Mathf.Clamp(multiplier, 1, maxMultiplier);
    }

    void UpdateGameplayUI()
    {
        if (gameplayUI != null)
            gameplayUI.UpdateGameplayUI(score, multiplier, GetCurrentRank());
    }

    string GetCurrentRank()
    {
        float percent = 0f;
        if (maxRawJudgementScore > 0)
            percent = (float)rawJudgementScore / maxRawJudgementScore * 100f;

        return GetRank(percent);
    }

    public void ShowFinalResults()
    {
        if (resultsShown) return;
        resultsShown = true;

        float percent = 0f;
        if (maxRawJudgementScore > 0)
            percent = (float)rawJudgementScore / maxRawJudgementScore * 100f;

        string rank = GetRank(percent);

        // Save the latest rhythm game rank so other scenes can use it
        RhythmResultData.latestDishRank = rank;

        if (resultsUI != null)
        {
            resultsUI.ShowResults(
                score,
                percent,
                rank,
                perfectCount,
                goodCount,
                earlyCount,
                missCount
            );
        }
    }

    string GetRank(float percent)
    {
        if (percent >= 95f) return "S";
        if (percent >= 85f) return "A";
        if (percent >= 75f) return "B";
        if (percent >= 65f) return "C";
        if (percent >= 50f) return "D";
        return "F";
    }
}