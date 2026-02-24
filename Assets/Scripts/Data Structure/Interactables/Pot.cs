using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Pot : Interactable
{
    public override void Use(GameObject plr)
    {
        SceneManager.LoadScene("Rhythm");
    }
}
