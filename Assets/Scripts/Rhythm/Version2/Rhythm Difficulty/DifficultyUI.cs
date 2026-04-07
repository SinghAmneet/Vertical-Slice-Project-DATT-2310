using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DifficultyUI : MonoBehaviour
{
    [Header("Reference")]
    public TMP_Text difficultyText;

    void Start()
    {
        UpdateDifficultyText();
    }

    void UpdateDifficultyText()
    {
        RhythmDifficulty difficulty = GetCurrentDifficulty();

        if (difficultyText == null)
            return;

        switch (difficulty)
        {
            case RhythmDifficulty.Easy:
                difficultyText.text = "Difficulty: <color=#7CFF7C>Easy</color>";
                break;

            case RhythmDifficulty.Medium:
                difficultyText.text = "Difficulty: <color=#FFD966>Medium</color>";
                break;

            case RhythmDifficulty.Hard:
                difficultyText.text = "Difficulty: <color=#FF6B6B>Hard</color>";
                break;
        }
    }

    RhythmDifficulty GetCurrentDifficulty()
    {
        switch (RhythmProgressData.rhythmRoundIndex)
        {
            case 0:
                return RhythmDifficulty.Easy;
            case 1:
                return RhythmDifficulty.Medium;
            default:
                return RhythmDifficulty.Hard;
        }
    }
}
