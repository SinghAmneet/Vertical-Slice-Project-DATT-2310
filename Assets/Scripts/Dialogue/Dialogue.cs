using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public enum DialogueState
{
    Typewriting,
    Waiting,
    Choosing,
}

public class Dialogue : MonoBehaviour
{
    [TextArea]
    public string tempDialogue;

    public string[] options;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Transform choiceBox;
    public GameObject continueText;
    public GameObject choicePrefab;

    public float typewriteRate = 0.1f;
    private float accum;

    private int letterIndex;
    private string currentLine;
    private string displayedText;

    private DialogueState state;

    private void Start()
    {
        ClearDisplay();
        state = DialogueState.Waiting;
    }

    private void Update()
    {
        switch(state)
        {
            case DialogueState.Typewriting:
                if (Input.GetKeyDown(KeyCode.Space)) TypewriteSkip();
                    Typewrite();
                break;
            case DialogueState.Waiting:
                if (Input.GetKeyDown(KeyCode.Space)) StartTypewrite("Demon king");
                break;
            case DialogueState.Choosing:
                break;
        }
    }

    private void DisplayText(string text)
    {
        dialogueText.text = text;
        displayedText = text;
    }

    private void ClearDisplay()
    {
        nameText.text = "";
        DisplayText("");
    }

    private void StartTypewrite(string name)
    {
        ClearDisplay();
        continueText.SetActive(false);
        letterIndex = 0;
        currentLine = tempDialogue;
        nameText.text = name;
        state = DialogueState.Typewriting;
    }

    private void TypewriteSkip()
    {
        DisplayText(currentLine);
        EndTypewrite();
    }

    private void EndTypewrite()
    {
        //state = DialogueState.Waiting;
        //continueText.SetActive(true);
        StartChoosing(options);
    }

    private void StartChoosing(string[] options)
    {
        DestroyOptions();
        for (int i = 0; i < options.Length; i++)
        {
            GameObject choice = Instantiate(choicePrefab, choiceBox);
            choice.GetComponent<DialogueChoice>().SetText(i, options[i], GetComponent<Dialogue>());
        }
        state = DialogueState.Choosing;
    }

    private void DestroyOptions()
    {
        for (int i = choiceBox.childCount - 1; i >= 0; i--)
        {
            Destroy(choiceBox.GetChild(i).gameObject);
        }
    }

    public void ChooseOption(int index)
    {
        DestroyOptions();
        Debug.Log("chose: " + options[index]);
        StartTypewrite("You");
    }

    private void Typewrite()
    {
        accum += Time.deltaTime;
        if (accum > typewriteRate)
        {
            accum = 0;
            char letter = currentLine[letterIndex];
            DisplayText(displayedText + letter);

            letterIndex++;

            if (letterIndex + 1 >= currentLine.Length) EndTypewrite();
        }
    }

}
