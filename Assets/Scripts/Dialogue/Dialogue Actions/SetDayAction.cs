using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Dialogue/Actions/Set Day")]
public class SetDayAction : DialogueAction
{
    public int day;
    public override void StartAction(Dialogue dialogue)
    {
        GameData.currentDay = day;
        GameData.hasDoneTutorial = true;
    }
}
