using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Score Values")]
    public int perfectPoints = 300;
    public int goodPoints = 150;
    public int latePoints = 50;

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
    private int lateCount = 0;
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

    // Returns true if the multiplier increased on this judgement
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

            case "LATE":
                lateCount++;
                combo++;
                UpdateMultiplier();
                score += latePoints * multiplier;
                rawJudgementScore += latePoints;
                break;

            case "MISS":
                missCount++;
                combo = 0;
                multiplier = 1;
                break;
        }

        Debug.Log($"Score: {score} | Combo: {combo} | Multiplier: x{multiplier}");
        UpdateGameplayUI();

        return multiplier > previousMultiplier;
    }

    // NEW: returns true if a miss dropped multiplier from 2x or more back to 1x
    public bool RegisterMissAndCheckMultiplierDrop()
    {
        int previousMultiplier = multiplier;

        missCount++;
        combo = 0;
        multiplier = 1;

        Debug.Log($"Score: {score} | Combo: {combo} | Multiplier: x{multiplier}");
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
        {
            gameplayUI.UpdateGameplayUI(score, multiplier, GetCurrentRank());
        }
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

        if (gameplayUI != null) gameplayUI.HideUI();

        float percent = 0f;
        if (maxRawJudgementScore > 0)
            percent = (float)rawJudgementScore / maxRawJudgementScore * 100f;

        string rank = GetRank(percent);

        Debug.Log(">RHYTHM GAME RESULTS<");
        Debug.Log($"Final Score: {score}");
        Debug.Log($"Accuracy: {percent:F1}%");
        Debug.Log($"Rank: {rank}");
        Debug.Log($"Perfect: {perfectCount}");
        Debug.Log($"Good: {goodCount}");
        Debug.Log($"Late: {lateCount}");
        Debug.Log($"Miss: {missCount}");

        if (resultsUI != null)
        {
            resultsUI.ShowResults(
                score,
                percent,
                rank,
                perfectCount,
                goodCount,
                lateCount,
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