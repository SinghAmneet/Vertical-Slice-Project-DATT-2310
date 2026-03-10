using UnityEngine;

[CreateAssetMenu(menuName ="Dialogue/Actions/Play Sound")]
public class SoundDialogueAction : DialogueAction
{
    public AudioClip audio;
    public bool waitUntilAudioFinishes;

    public override void StartAction(Dialogue dialogue)
    {
        dialogue.audioSource.clip = audio;
        dialogue.audioSource.Play();
        //dialogue.audioSource.PlayOneShot(audio);

        if (waitUntilAudioFinishes) dialogue.WaitForAudioToFinish(dialogue.audioSource.clip.length);
    }
}
