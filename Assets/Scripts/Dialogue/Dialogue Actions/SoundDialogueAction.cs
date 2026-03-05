using UnityEngine;

[CreateAssetMenu(menuName ="Dialogue/Actions/Play Sound")]
public class SoundDialogueAction : DialogueAction
{
    public AudioSource sound;

    public override void StartAction()
    {
        sound.Play();
    }
}
