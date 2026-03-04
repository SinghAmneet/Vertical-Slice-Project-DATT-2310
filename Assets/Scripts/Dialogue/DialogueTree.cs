using System;
using UnityEngine;

public enum Character
{
    Narrator,
    Kenny,
    Flint,
}

[CreateAssetMenu(menuName ="Dialogue/Dialogue Tree")]
public class DialogueTree : ScriptableObject
{
    public DialogueData[] dialogueLines; // dialogue lines
    public DialogueChoice[] endChoices; // choices after the dialogue lines
}

[Serializable]
public class DialogueData
{
    public Character speaker;

    [TextArea]
    public string dialogue;
    public DialogueAction endAction; // action that happens at the end of the dialogue
}

[Serializable]
public class DialogueChoice
{
    public string choiceDialogue;
    public DialogueTree choiceDialogueTree; // the tree to go to after selecting this choice
    public DialogueAction choiceAction; // the action to do after selecting this choice
}