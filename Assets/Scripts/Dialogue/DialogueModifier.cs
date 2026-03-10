
public abstract class DialogueModifier
{
    protected string tagName;
    protected Dialogue dialogue;

    public DialogueModifier(Dialogue dialogue, string tagName)
    {
        this.dialogue = dialogue;
        this.tagName = tagName;
    }

    public bool Equals(string name)
    {
        return tagName.Equals(name);
    }

    public abstract void ApplyModifier(string value);
}

public class PauseModifier : DialogueModifier
{
    public PauseModifier(Dialogue dialogue) : base(dialogue, "Pause") { }

    public override void ApplyModifier(string value)
    {
        float timer = value.Equals("") ? -1 : float.Parse(value); // convert to float
        dialogue.Pause(timer);
    }
}

public class RateModifier : DialogueModifier
{
    public RateModifier(Dialogue dialogue) : base(dialogue, "Rate") { }

    public override void ApplyModifier(string value)
    {
        float rate = value.Equals("Default") ? dialogue.typewriteRate : float.Parse(value); // convert to float
        dialogue.SetRate(rate);
    }

}