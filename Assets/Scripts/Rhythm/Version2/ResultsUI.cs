using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class IngredientSpriteEntry
{
    public string ingredientName;
    public Sprite sprite;
}

[System.Serializable]
public class DishSpriteEntry
{
    public string dishName;
    public Sprite sprite;
}

public class ResultsUI : MonoBehaviour
{
    [Header("References")]
    public GameplayUI gameplayUI;
    public CursorManager cursorManager;

    [Header("Panel")]
    public GameObject resultsPanel;
    public TMP_Text finishedText;

    [Header("Text Fields")]
    public TMP_Text rankText;
    public TMP_Text scoreText;
    public TMP_Text accuracyText;
    public TMP_Text perfectText;
    public TMP_Text goodText;
    public TMP_Text earlyText;
    public TMP_Text missText;
    public TMP_Text finalStatsText;

    [Header("Optional Label Text")]
    public TMP_Text dishText;
    public TMP_Text ingredientsText;

    [Header("Final Dish Visual")]
    public Image finalDishImage;

    [Header("Ingredient Slot Visuals (max 6)")]
    public Image[] ingredientSlotImages = new Image[6];

    [Header("Sprite Lookup")]
    public List<IngredientSpriteEntry> ingredientSpriteLookup = new List<IngredientSpriteEntry>();
    public List<DishSpriteEntry> dishSpriteLookup = new List<DishSpriteEntry>();

    [Header("Fallback")]
    public Sprite missingIngredientSprite;
    public Sprite missingDishSprite;

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
        int early,
        int miss)
    {
        StartCoroutine(ShowResultsRoutine(score, accuracy, rank, perfect, good, early, miss));
    }

    IEnumerator ShowResultsRoutine(
        int score,
        float accuracy,
        string rank,
        int perfect,
        int good,
        int early,
        int miss)
    {
        if (gameplayUI != null)
        {
            gameplayUI.HideUI();
            yield return new WaitForSeconds(gameplayUIFadeOutWait);
        }

        if (cursorManager != null)
            cursorManager.SetGameplayCursorActive(false);

        if (finishedText != null)
        {
            finishedText.gameObject.SetActive(true);
            finishedText.alpha = 1f;
        }

        yield return new WaitForSeconds(finishedDisplayTime);

        if (finishedText != null)
            finishedText.gameObject.SetActive(false);

        if (resultsPanel != null)
            resultsPanel.SetActive(true);

        SetUIActive(false);
        ClearIngredientSlots();
        ClearDishImage();

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
            rankText.text = "Dish Rank: " + rank;
            rankText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        if (dishText != null)
        {
            // if (DishData.createdDish != null)
            //     dishText.text = "Final Dish:";
            dishText.text = "Final Dish:";

            dishText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        ShowFinalDishSprite();

        if (ingredientsText != null)
        {
            ingredientsText.text = "Ingredients:";
            ingredientsText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        ShowIngredientSprites();

        if (finalStatsText != null)
        {
            finalStatsText.text = "Stats: " + GetStats();
            finalStatsText.gameObject.SetActive(true);
        }

        if (scoreText != null)
        {
            scoreText.text = "Rhythm Score: " + score.ToString();
            scoreText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        if (accuracyText != null)
        {
            accuracyText.text = "Note Accuracy: " + accuracy.ToString("F1") + "%";
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

        if (earlyText != null)
        {
            earlyText.text = "Early Notes: " + early;
            earlyText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        if (missText != null)
        {
            missText.text = "Missed Notes: " + miss;
            missText.gameObject.SetActive(true);
            yield return new WaitForSeconds(lineRevealDelay);
        }

        yield return new WaitForSeconds(0.25f);

        if (startMenuButton != null) startMenuButton.SetActive(true);
        if (mainSceneButton != null) mainSceneButton.SetActive(true);
    }

    void SetUIActive(bool state)
    {
        if (rankText != null) rankText.gameObject.SetActive(state);
        if (dishText != null) dishText.gameObject.SetActive(state);

        if (finalDishImage != null)
            finalDishImage.gameObject.SetActive(state);

        if (ingredientsText != null) ingredientsText.gameObject.SetActive(state);

        if (ingredientSlotImages != null)
        {
            for (int i = 0; i < ingredientSlotImages.Length; i++)
            {
                if (ingredientSlotImages[i] != null)
                    ingredientSlotImages[i].gameObject.SetActive(state);
            }
        }

        if (finalStatsText != null) finalStatsText.gameObject.SetActive(state);
        if (scoreText != null) scoreText.gameObject.SetActive(state);
        if (accuracyText != null) accuracyText.gameObject.SetActive(state);
        if (perfectText != null) perfectText.gameObject.SetActive(state);
        if (goodText != null) goodText.gameObject.SetActive(state);
        if (earlyText != null) earlyText.gameObject.SetActive(state);
        if (missText != null) missText.gameObject.SetActive(state);

    }

    void ShowFinalDishSprite()
    {
        if (finalDishImage == null)
            return;

        finalDishImage.gameObject.SetActive(true);

        if (DishData.createdDish == null)
        {
            finalDishImage.sprite = missingDishSprite;
            finalDishImage.enabled = finalDishImage.sprite != null;
            return;
        }

        string dishName = DishData.createdDish.dishName;
        Sprite dishSprite = GetDishSpriteByName(dishName);

        if (dishSprite == null)
            dishSprite = missingDishSprite;

        finalDishImage.sprite = dishSprite;
        finalDishImage.enabled = finalDishImage.sprite != null;
    }

    void ShowIngredientSprites()
    {
        ClearIngredientSlots();

        if (DishData.inventory == null || DishData.inventory.Count == 0)
            return;

        int maxSlots = Mathf.Min(ingredientSlotImages.Length, DishData.inventory.Count);

        for (int i = 0; i < maxSlots; i++)
        {
            if (ingredientSlotImages[i] == null)
                continue;

            ingredientSlotImages[i].gameObject.SetActive(true);

            FoodData food = DishData.inventory[i];
            Sprite ingredientSprite = null;

            if (food != null)
                ingredientSprite = GetIngredientSpriteByName(food.name);

            if (ingredientSprite == null)
                ingredientSprite = missingIngredientSprite;

            ingredientSlotImages[i].sprite = ingredientSprite;
            ingredientSlotImages[i].enabled = ingredientSlotImages[i].sprite != null;
        }
    }

    void ClearIngredientSlots()
    {
        if (ingredientSlotImages == null)
            return;

        for (int i = 0; i < ingredientSlotImages.Length; i++)
        {
            if (ingredientSlotImages[i] == null)
                continue;

            ingredientSlotImages[i].sprite = null;
            ingredientSlotImages[i].enabled = false;
            ingredientSlotImages[i].gameObject.SetActive(false);
        }
    }

    void ClearDishImage()
    {
        if (finalDishImage == null)
            return;

        finalDishImage.sprite = null;
        finalDishImage.enabled = false;
        finalDishImage.gameObject.SetActive(false);
    }

    Sprite GetIngredientSpriteByName(string ingredientName)
    {
        for (int i = 0; i < ingredientSpriteLookup.Count; i++)
        {
            if (ingredientSpriteLookup[i] != null &&
                ingredientSpriteLookup[i].ingredientName == ingredientName)
            {
                return ingredientSpriteLookup[i].sprite;
            }
        }

        return null;
    }

    Sprite GetDishSpriteByName(string dishName)
    {
        for (int i = 0; i < dishSpriteLookup.Count; i++)
        {
            if (dishSpriteLookup[i] != null &&
                dishSpriteLookup[i].dishName == dishName)
            {
                return dishSpriteLookup[i].sprite;
            }
        }

        return null;
    }

    public string GetStats()
    {
        if (DishData.totalStats == null || DishData.totalStats.Count == 0)
            return "None";

        string str = "";
        int count = 0;

        foreach (var pair in DishData.totalStats)
        {
            str += $"{pair.Key}: {pair.Value}";
            count++;

            if (count < DishData.totalStats.Count)
                str += ", ";
        }

        return str;
    }

    public void GoToStartMenu()
    {
        RhythmProgressData.rhythmRoundIndex = 0;
        SceneManager.LoadScene("StartMenu");
    }

    public void RestartMain()
    {
        RhythmProgressData.rhythmRoundIndex++;
        SceneManager.LoadScene("MainScene");
    }
}