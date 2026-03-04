using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName ="Dialogue/Actions/Load Scene")]
public class LoadSceneDialogueAction : DialogueAction
{
    public string sceneName;

    public override void StartAction()
    {
        SceneManager.LoadScene(sceneName);
    }
}
