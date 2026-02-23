using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public AudioSource bgm;

    public Sprite muteOn;
    public Sprite muteOff;
    public GameObject muteButton;

    private List<GameObject> hudChildren = new();

    private void Awake()
    {
        menu.SetActive(false);

        // get all children in HUD excluding pause menu and cheat menu
        foreach (Transform child in menu.transform.parent.transform)
        {
            if (child == menu.transform || child.name == "Cheat Menu") continue;
            hudChildren.Add(child.gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }

        if (Input.GetKeyUp(KeyCode.M)) 
        { 
            ToggleMute();
        }
    }

    public void ToggleMute()
    {
        bgm.mute = !bgm.mute;
        muteButton.GetComponent<Image>().sprite = (bgm.mute ? muteOn : muteOff);
    }

    public void AudioChanged(float value)
    {
        bgm.volume = value;
    }

    public void ToggleMenu()
    {
        menu.SetActive(!menu.activeSelf);

        // update active states for all children in HUD
        foreach (GameObject child in hudChildren)
        {
            child.SetActive(!menu.activeSelf);
        }

        Time.timeScale = menu.activeSelf ? 0f : 1f;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
