using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName ="Dialogue/Actions/Load Scene")]
public class LoadSceneDialogueAction : DialogueAction
{
    public string sceneName;
    public endings ending;

    public override void StartAction(Dialogue dialogue)
    {
        if (ending != endings.None) EndingData.currentEnding = ending;
        SceneManager.LoadScene(sceneName);
    }
}
