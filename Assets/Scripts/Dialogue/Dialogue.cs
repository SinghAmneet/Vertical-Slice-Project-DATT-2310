using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;

public enum DialogueState
{
    Start,
    Typewriting,
    Waiting,
    Skipping,
    Paused,
    Choosing,
}

public class Dialogue : MonoBehaviour
{
    public DialogueTree startingDialogueTree;
    private DialogueTree currentTree;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Transform choiceBox;
    public GameObject continueText;
    public GameObject choicePrefab;

    public Color thinkingTextColor;

    public float typewriteRate = 0.1f; // write one letter per the provided rate
    private float currentRate;
    private float accum;

    private int dialogueIndex;
    private int letterIndex;
    private string currentLine;
    private StringBuilder displayedText = new();

    private DialogueState state;

    private DialogueModifier[] modifiers = new DialogueModifier[2];
    private char modifierStart = '[';
    private char modifierEnd = ']';
    private char valueChar = ':';

    private float pauseTimer;

    private void Awake()
    {
        modifiers[0] = new PauseModifier(this);
        modifiers[1] = new RateModifier(this);
    }

    private void Start()
    {
        ClearDisplay();
        currentTree = startingDialogueTree;
        currentRate = typewriteRate;
        NextDialogueLine();
    }

    private void Update()
    {
        switch(state)
        {
            case DialogueState.Typewriting:
                if (Input.GetKeyDown(KeyCode.Space)) SkipTypewrite();
                    Typewrite();
                break;
            case DialogueState.Waiting:
                if (Input.GetKeyDown(KeyCode.Space)) NextDialogueLine();
                break;
            case DialogueState.Paused:
                if (Input.GetKeyDown(KeyCode.Space)) Resume();
                decreasePauseTimer();
                break;
            case DialogueState.Choosing:
                break;
        }
    }

    public void SetRate(float rate)
    {
        currentRate = rate;
    }

    private void DisplayText(string text)
    {
        dialogueText.text = text;
    }

    private void ClearDisplay()
    {
        nameText.text = "";
        DisplayText("");
        ResetTextBox();
        displayedText.Clear();
    }

    private void ResetTextBox()
    {
        dialogueText.fontStyle = FontStyles.Normal;
        dialogueText.color = Color.white;
    }

    private void UpdateTextBox()
    {
        dialogueText.fontStyle = FontStyles.Italic;
        dialogueText.color = Color.gray;
    }

    private string UpdateName(string name)
    {
        if (name.Equals("Flint"))
        {
            return "King Flint";
        } else { return name; }
    }

    // go to next dialogue line in the dialogue tree
    private void NextDialogueLine()
    {
        ClearDisplay();

        DialogueData data = currentTree.dialogueLines[dialogueIndex];
        currentLine = data.dialogue;
        nameText.text = UpdateName(data.speaker.ToString());
        dialogueIndex++;

        if (data.speech == Speech.Thinking) UpdateTextBox();
        if (data.startAction != null) data.startAction.StartAction();

        StartTypewrite();
    }

    // write a letter to the text box per rate
    private void Typewrite()
    {
        accum += Time.deltaTime;
        if (accum > currentRate)
        {
            accum = 0;
            displayedText.Append(currentLine[letterIndex]);
            DisplayText(displayedText.ToString());

            letterIndex++;

            if (letterIndex >= currentLine.Length)
            {
                EndTypewrite();
            } else
            {
                ApplyModifier(currentLine[letterIndex]);
            }
        }
    }

    // apply dialogue modifier
    private void ApplyModifier(char letter)
    {
        // if next letter is not the start of a modifier tag
        if (!letter.Equals(modifierStart)) return;

        // get the end of the modifier tag
        int endIndex = currentLine.IndexOf(modifierEnd, letterIndex);

        // get modifier name
        string tagName = currentLine.Substring(letterIndex + 1, endIndex - (letterIndex + 1));
        int valueIndex = tagName.IndexOf(valueChar); // get modifier value
        string value;
        
        letterIndex = endIndex + 1;
        
        // if value was not found
        if (valueIndex < 0)
        {
            value = "";
        } else
        {
            value = tagName.Substring(valueIndex + 1);
            tagName = tagName.Substring(0, valueIndex);
        }
        
        // find the modifier and apply
        foreach (DialogueModifier mod in modifiers)
        {
            if (mod.Equals(tagName))
            {
                mod.ApplyModifier(value);
                break;
            }
        }
    }

    // set up typewriting
    private void StartTypewrite()
    {
        continueText.SetActive(false);

        SetRate(typewriteRate);
        letterIndex = 0;
        state = DialogueState.Typewriting;
    }

    // write the entire current dialogue instantly
    private void SkipTypewrite()
    {
        SetRate(0);
    }

    // stop type writing
    private void EndTypewrite()
    {
        DialogueData data = currentTree.dialogueLines[dialogueIndex - 1];
        if (data.endAction != null) data.endAction.StartAction();

        // if reached end of dialogue lines
        if (dialogueIndex >= currentTree.dialogueLines.Length)
        {
            StartChoosing();
        } else
        {
            continueText.SetActive(true);
            state = DialogueState.Waiting;
        }
    }

    public void Pause()
    {
        continueText.SetActive(true);
        state = DialogueState.Paused;
    }

    public void Pause(float timer)
    {
        pauseTimer = timer;
        Pause();
    }

    public void decreasePauseTimer()
    {
        if (pauseTimer < 0) return;
        pauseTimer = Mathf.Max(pauseTimer - Time.deltaTime, 0);
        if (pauseTimer == 0) Resume();
    }

    private void Resume()
    {
        continueText.SetActive(false);
        state = DialogueState.Typewriting;
    }

    // create option objects
    private void StartChoosing()
    {
        DestroyOptions();
        for (int i = 0; i < currentTree.endChoices.Length; i++)
        {
            GameObject choice = Instantiate(choicePrefab, choiceBox);
            choice.GetComponent<DialogueChoiceUI>().SetText(i, currentTree.endChoices[i].choiceDialogue, this);
        }
        state = DialogueState.Choosing;
    }

    // destroy all option objects
    private void DestroyOptions()
    {
        for (int i = choiceBox.childCount - 1; i >= 0; i--)
        {
            Destroy(choiceBox.GetChild(i).gameObject);
        }
    }

    // choose the clicked option depending on its index
    public void ChooseOption(int index)
    {
        DestroyOptions();
        DialogueChoice data = currentTree.endChoices[index];
        currentTree = data.choiceDialogueTree;

        if (data.choiceAction != null)
        {
            data.choiceAction.StartAction();
            if (data.choiceAction is LoadSceneDialogueAction) return;
        }

        dialogueIndex = 0;
        NextDialogueLine();
    }

    public void goToGame()
    {
        SceneManager.LoadScene("MainScene");
    }

}
