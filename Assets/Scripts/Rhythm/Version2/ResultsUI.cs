using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultsUI : MonoBehaviour
{
    [Header("Panel")]
    public GameObject resultsPanel;

    [Header("Text Fields")]
    public TMP_Text rankText;
    public TMP_Text scoreText;
    public TMP_Text accuracyText;

    public TMP_Text perfectText;
    public TMP_Text goodText;
    public TMP_Text lateText;
    public TMP_Text missText;

    public void ShowResults(
        int score,
        float accuracy,
        string rank,
        int perfect,
        int good,
        int late,
        int miss)
    {
        resultsPanel.SetActive(true);

        rankText.text = "Rank: " + rank;
        scoreText.text = "Score: " + score.ToString();
        accuracyText.text = "Accuracy: " + accuracy.ToString("F1") + "%";

        perfectText.text = "Perfect: " + perfect;
        goodText.text = "Good: " + good;
        lateText.text = "Late: " + late;
        missText.text = "Missed: " + miss;
    }
}
