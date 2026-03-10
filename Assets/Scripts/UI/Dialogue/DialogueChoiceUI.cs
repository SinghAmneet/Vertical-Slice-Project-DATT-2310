using TMPro;
using UnityEngine;

public class DialogueChoiceUI : MonoBehaviour
{
    private int indexOption;
    private Dialogue dialogue;
    private TextMeshProUGUI choiceText;

    public void ChooseOption()
    {
        dialogue.ChooseOption(indexOption);
    }

    private void Awake()
    {
        choiceText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public void SetText(int index, string text, Dialogue dialogue)
    {
        choiceText.text = text;
        gameObject.name = index.ToString();
        indexOption = index;
        this.dialogue = dialogue;
    }
}
