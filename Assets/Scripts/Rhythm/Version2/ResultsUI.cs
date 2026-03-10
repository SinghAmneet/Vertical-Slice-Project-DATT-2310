using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultsUI : MonoBehaviour
{
    [Header("References")]
    public GameplayUI gameplayUI;

    [Header("Panel")]
    public GameObject resultsPanel;
    public TMP_Text finishedText;

    [Header("Text Fields")]
    public TMP_Text rankText;
    public TMP_Text scoreText;
    public TMP_Text accuracyText;
    public TMP_Text perfectText;
    public TMP_Text goodText;
    public TMP_Text lateText;
    public TMP_Text missText;
    public TMP_Text dishText;
    public TMP_Text ingredientsText;
    public TMP_Text finalStatsText;
    
    [Header("Buttons")]
    public GameObject startMenuButton;
    public GameObject mainSceneButton;

    [Header("Timing")]
    public float gameplayUIFadeOutWait = 0.65f;
    public float finishedDisplayTime = 1.2f;
    public float panelFadeDuration = 0.6f;
    public float lineRevealDelay = 0.3f;

    private CanvasGroup panelCanvasGroup;

    void Awake()
    {
        if (startMenuButton != null) startMenuButton.SetActive(false);

        if (mainSceneButton != null) mainSceneButton.SetActive(false);

        if (resultsPanel != null)
        {
            panelCanvasGroup = resultsPanel.GetComponent<CanvasGroup>();

            if (panelCanvasGroup == null)
                panelCanvasGroup = resultsPanel.AddComponent<CanvasGroup>();

            resultsPanel.SetActive(false);
            panelCanvasGroup.alpha = 0f;
        }

        if (finishedText != null)
            finishedText.gameObject.SetActive(false);
    }

    public void ShowResults(
        int score,
        float accuracy,
        string rank,
        int perfect,
        int good,
        int late,
        int miss)
    {
        StartCoroutine(ShowResultsRoutine(score, accuracy, rank, perfect, good, late, miss));
    }

    IEnumerator ShowResultsRoutine(
        int score,
        float accuracy,
        string rank,
        int perfect,
        int good,
        int late,
        int miss)
    {
        // Fade out gameplay HUD first
        if (gameplayUI != null)
        {
            gameplayUI.HideUI();
            yield return new WaitForSeconds(gameplayUIFadeOutWait);
        }

        // Show FINISHED
        if (finishedText != null)
        {
            finishedText.gameObject.SetActive(true);
            finishedText.alpha = 1f;
        }

        yield return new WaitForSeconds(finishedDisplayTime);

        if (finishedText != null)
            finishedText.gameObject.SetActive(false);

        // Show panel
        if (resultsPanel != null)
            resultsPanel.SetActive(true);

        SetTextActive(false);

        float timer = 0f;
        while (timer < panelFadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / panelFadeDuration;

            if (panelCanvasGroup != null)
                panelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        if (panelCanvasGroup != null)
            panelCanvasGroup.alpha = 1f;

        if (rankText != null)
        {
            rankText.text = "Dish rank: " + rank;
            rankText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        if (scoreText != null)
        {
            scoreText.text = "Rhythm Score: " + score.ToString();
            scoreText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        if (accuracyText != null)
        {
            accuracyText.text = "Note Accuracy: " + accuracy.ToString("F1") + " per.";
            accuracyText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        if (perfectText != null)
        {
            perfectText.text = "Perfect Notes: " + perfect;
            perfectText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        if (goodText != null)
        {
            goodText.text = "Good Notes: " + good;
            goodText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        if (lateText != null)
        {
            lateText.text = "Late Notes: " + late;
            lateText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        if (missText != null)
        {
            missText.text = "Missed Notes: " + miss;
            missText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        if (dishText != null)
        {
            dishText.text = "Final Dish: " + DishData.createdDish.dishName;
            dishText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        if (ingredientsText != null)
        {
            ingredientsText.text = "Ingredients: " + GetIngredients();
            ingredientsText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        if (finalStatsText != null)
        {
            finalStatsText.text = "Stats: " + GetStats();
            finalStatsText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.25f);

        if (startMenuButton != null) startMenuButton.SetActive(true);

        if (mainSceneButton != null) mainSceneButton.SetActive(true);
    }

    void SetTextActive(bool state)
    {
        if (rankText != null) rankText.gameObject.SetActive(state);
        if (scoreText != null) scoreText.gameObject.SetActive(state);
        if (accuracyText != null) accuracyText.gameObject.SetActive(state);
        if (perfectText != null) perfectText.gameObject.SetActive(state);
        if (goodText != null) goodText.gameObject.SetActive(state);
        if (lateText != null) lateText.gameObject.SetActive(state);
        if (missText != null) missText.gameObject.SetActive(state);
        if (dishText != null) dishText.gameObject.SetActive(state);
        if (ingredientsText != null) ingredientsText.gameObject.SetActive(state);
        if (finalStatsText != null) finalStatsText.gameObject.SetActive(state);
    }

    public string GetIngredients()
    {
        string str = "";

        for (int i = 0; i < DishData.inventory.Count; i++) 
        {
            FoodData food = DishData.inventory[i];
            str += food.name;
            if (i + 1 != DishData.inventory.Count) str += ", ";
        }
        return str;
    }

    public string GetStats()
    {
        string str = "";

        foreach (var(stat, value) in DishData.totalStats)
        {
            str += $"{stat.ToString()}: {value}, ";
        }
        return str;
    }

    public void GoToStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void RestartMain()
    {
        SceneManager.LoadScene("MainScene");
    }
}