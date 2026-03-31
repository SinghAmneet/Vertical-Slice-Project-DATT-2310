
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Actions/Rank Eval Dialogue")]
public class GetEvalDialogue : DialogueAction
{
    public DialogueTree saGrade;
    public DialogueTree bcGrade;
    public DialogueTree fGrade;

    public override void StartAction(Dialogue dialogue)
    {
        dialogue.EnableDialogueOverride();

        string rank = RhythmResultData.latestDishRank;

        if (rank == "S" || rank == "A")
        {
            dialogue.NextTree(saGrade);
        } else if (rank == "B" || rank == "C")
        {
            dialogue.NextTree(bcGrade);
        } else
        {
            dialogue.NextTree(fGrade);
        }
    }
}
