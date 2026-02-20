using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    public AudioSource bgm;

    private List<GameObject> hudChildren = new();

    private void Awake()
    {
        menu.SetActive(false);

        // get all children in HUD excluding pause menu
        foreach (Transform child in menu.transform.parent.transform)
        {
            if (child == menu.transform) continue;
            hudChildren.Add(child.gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
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

        if (menu.activeSelf)
        {
            Time.timeScale = 0f; // pause time
        } else
        {
            Time.timeScale = 1f; // resume time
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
