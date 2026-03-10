using System;
using UnityEngine;

public enum Character
{
    Narrator,
    Kenny,
    Flint,
}

public enum Speech
{
    Talking,
    Thinking,
}

[CreateAssetMenu(menuName ="Dialogue/Dialogue Tree")]
public class DialogueTree : ScriptableObject
{
    public DialogueData[] dialogueLines; // dialogue lines
    public DialogueChoice[] endChoices; // choices after the dialogue lines
    public DialogueTree nextTree; // next dialogue tree if there are no choices
}

[Serializable]
public class DialogueData
{
    public Character speaker;
    public Speech speech;

    [TextArea]
    public string dialogue;
    public DialogueAction startAction; // action that happens at the start of the dialogue
    public DialogueAction endAction; // action that happens at the end of the dialogue
}

[Serializable]
public class DialogueChoice
{
    public string choiceDialogue;
    public DialogueTree choiceDialogueTree; // the tree to go to after selecting this choice
    public DialogueAction choiceAction; // the action to do after selecting this choice
}