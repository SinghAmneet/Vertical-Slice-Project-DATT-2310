using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RhythmProgressData
{
    // 0 = Easy, 1 = Medium, 2+ = Hard
    public static int rhythmRoundIndex = 0;

    public static RhythmDifficulty GetCurrentDifficulty()
    {
        switch (rhythmRoundIndex)
        {
            case 0:
                return RhythmDifficulty.Easy;
            case 1:
                return RhythmDifficulty.Medium;
            default:
                return RhythmDifficulty.Hard;
        }
    }

    public static void AdvanceRhythmRound()
    {
        rhythmRoundIndex++;
    }

    // Reset static game data
    public static void ResetGameProgress()
    {
        rhythmRoundIndex = 0;
        RhythmResultData.latestDishRank = "F";
    }
}

// public class RhythmProgressData : MonoBehaviour
// {
//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
