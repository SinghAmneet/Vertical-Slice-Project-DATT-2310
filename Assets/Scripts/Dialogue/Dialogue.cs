using UnityEngine;
using TMPro;
using System.Text;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public enum DialogueState
{
    Standby,
    Typewriting,
    Waiting,
    Skipping,
    Paused,
    Choosing,
    ChangingScene,
}

public class Dialogue : MonoBehaviour
{
    public DialogueTree startingDialogueTree;
    private DialogueTree currentTree;
    public DialogueTree restartTree;

    public AudioSource vaSound;
    public AudioSource audioSource;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public Transform choiceBox;
    public GameObject continueText;
    public GameObject choicePrefab;

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

    private Coroutine audioPlaying;
    private float pauseTimer;

    private bool overrideDialogue;

    private void Awake()
    {
        modifiers[0] = new PauseModifier(this);
        modifiers[1] = new RateModifier(this);
    }

    private void Start()
    {
        Time.timeScale = 1;
        ClearDisplay();
        currentTree = startingDialogueTree;

        if (EndingData.currentEnding == endings.WrongDialogue) currentTree = restartTree;
        EndingData.currentEnding = endings.None;

        currentRate = typewriteRate;
        NextDialogueLine();
    }

    private void Update()
    {
        switch(state)
        {
            case DialogueState.Typewriting:
                if (Input.GetButtonDown("Attack")) SkipTypewrite();
                    Typewrite();
                break;
            case DialogueState.Waiting:
                if (Input.GetButtonDown("Attack")) NextDialogueLine();
                break;
            case DialogueState.Paused:
                if (Input.GetButtonDown("Attack")) Resume();
                decreasePauseTimer();
                break;
            case DialogueState.Choosing:
                break;
            case DialogueState.ChangingScene:
                if (Input.GetButtonDown("Attack")) ChangeScene();
                break;
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            dialogueIndex = currentTree.dialogueLines.Length - 1;
            NextDialogueLine(); }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RhythmResultData.latestDishRank = "S";
            Debug.Log("Set Rank to: S");
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RhythmResultData.latestDishRank = "B";
            Debug.Log("Set Rank to: B");
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RhythmResultData.latestDishRank = "F";
            Debug.Log("Set Rank to: F");
        }
    }

    public void WaitForAudioToFinish(float audioLength)
    {
        audioPlaying = StartCoroutine(ResumeDialogue(audioLength));
    }

    IEnumerator ResumeDialogue(float audioLength)
    {
        yield return new WaitForSeconds(audioLength);
        continueText.SetActive(true);
        audioPlaying = null;
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
        if (audioPlaying != null) { 
            StopCoroutine(audioPlaying); 
            audioPlaying = null;
            audioSource.Stop();
        }
        ClearDisplay();

        DialogueData data = currentTree.dialogueLines[dialogueIndex];
        currentLine = data.dialogue;
        nameText.text = UpdateName(data.speaker.ToString());
        dialogueIndex++;

        if (data.speech == Speech.Thinking) UpdateTextBox();
        if (data.startAction != null) data.startAction.StartAction(this);

        if (vaSound.isPlaying) vaSound.Stop();

        if (data.va != null)
        {
            vaSound.clip = data.va;
            vaSound.Play();
        }

        StartTypewrite();
    }

    // write a letter to the text box per rate
    private void Typewrite()
    {
        accum += Time.deltaTime;
        if (accum > currentRate)
        {
            if (letterIndex >= currentLine.Length)
            {
                EndTypewrite();
                return;
            }

            if (currentLine[letterIndex].Equals(modifierStart))
            {
                // apply modifiers until the letter isn't the start of a modifier
                while (letterIndex < currentLine.Length &&
                    currentLine[letterIndex].Equals(modifierStart)
                    )
                {
                    ApplyModifier();
                }
            } else
            {
                accum = 0;
                displayedText.Append(currentLine[letterIndex]);
                DisplayText(displayedText.ToString());
                letterIndex++;

                if (letterIndex >= currentLine.Length)
                {
                    EndTypewrite();
                }
            }
        }
    }

    // apply dialogue modifier
    private void ApplyModifier()
    {
        // get the end of the modifier tag
        int endIndex = currentLine.IndexOf(modifierEnd, letterIndex);
        // get modifier name
        string tagName = currentLine.Substring(letterIndex + 1, endIndex - (letterIndex + 1));
        int valueIndex = tagName.IndexOf(valueChar); // get modifier value
        string value;
        
        letterIndex = endIndex + 1;
        if (currentRate == 0) return;

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
    public void EndTypewrite()
    {
        //Debug.Log($"{dialogueIndex} {currentTree.dialogueLines.Length}");
        DialogueData data = currentTree.dialogueLines[dialogueIndex - 1];
        if (data.endAction != null) StartEndAction(data);
        // if reached end of dialogue lines of the current dialogue tree
        if (dialogueIndex >= currentTree.dialogueLines.Length)
        {
            if (state == DialogueState.ChangingScene) return;

            // if there are end choices
            if (currentTree.endChoices.Length > 0)
            {
                StartChoosing();
            } else
            {
                if (overrideDialogue)
                {
                    overrideDialogue = false;
                } else {
                    NextTree(currentTree.nextTree);
                }
                
            }
                
        } else
        {
            state = DialogueState.Waiting;
            if (audioPlaying != null) return;
            continueText.SetActive(true);
        }
    }

    public void Pause()
    {
        if (currentRate == 0) return;
        continueText.SetActive(true);
        Pause(-1f);
    }

    public void Pause(float timer)
    {
        if (currentRate == 0) return;
        pauseTimer = timer;
        state = DialogueState.Paused;
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

        if (pauseTimer > 0) SkipTypewrite();
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
            data.choiceAction.StartAction(this);
            if (data.choiceAction is LoadSceneDialogueAction) return;
        }

        dialogueIndex = 0;
        NextDialogueLine();
    }

    public void EnableDialogueOverride()
    {
        overrideDialogue = true;
    }

    public void NextTree(DialogueTree tree)
    {
        continueText.SetActive(true);
        overrideDialogue = false;
        currentTree = tree;
        dialogueIndex = 0;
        state = DialogueState.Waiting;
    }

    private void StartEndAction(DialogueData data)
    {
        if (data.endAction is LoadSceneDialogueAction)
        {
            continueText.SetActive(true);
            state = DialogueState.ChangingScene;
        } else
        {
            data.endAction.StartAction(this);
        }
    }

    private void ChangeScene()
    {
        if (dialogueIndex >= currentTree.dialogueLines.Length)
        {
            DialogueData data = currentTree.dialogueLines[dialogueIndex - 1];
            if (data.endAction != null) data.endAction.StartAction(this);
        }
    }

    public void goToGame()
    {
        SceneManager.LoadScene("MainScene");
    }

}
