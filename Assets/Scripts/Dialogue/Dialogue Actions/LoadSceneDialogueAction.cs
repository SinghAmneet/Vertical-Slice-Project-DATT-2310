using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName ="Dialogue/Actions/Load Scene")]
public class LoadSceneDialogueAction : DialogueAction
{
    public string sceneName;
    public endings ending;
    public bool useLoadingScreen;

    public override void StartAction(Dialogue dialogue)
    {
        if (ending != endings.None) EndingData.currentEnding = ending;
        if (useLoadingScreen)
        {
            SceneLoader.LoadScene(sceneName);
        } else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
