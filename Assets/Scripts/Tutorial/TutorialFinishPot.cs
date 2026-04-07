using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialFinishPot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TutorialManager tutorialManager;
    [SerializeField] private GameObject confirmPanel;

    [Header("Scene")]
    [SerializeField] private string nextSceneName = "MainScene";

    private bool playerInRange;

    private void Start()
    {
        if (confirmPanel != null)
        {
            confirmPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (!playerInRange) return;
        if (tutorialManager == null) return;
        if (tutorialManager.GetCurrentStep() != 4) return;

        //if (Input.GetKeyDown(KeyCode.F))
        if (Input.GetButtonDown("Pickup"))
        {
            Debug.Log("F pressed at pot");
            tutorialManager.HideTutorialText();

            if (confirmPanel != null)
            {
                confirmPanel.SetActive(true);
                Time.timeScale = 0f;
            }
        }
    }

    public void ConfirmFinishTutorial()
    {
        Time.timeScale = 1f;
        GameData.hasDoneTutorial = true; // letting the game know that player has finished the tutorial
        SceneLoader.LoadScene(nextSceneName);
    }

    public void CancelFinishTutorial()
    {
        Time.timeScale = 1f;

        if (confirmPanel != null)
        {
            confirmPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered pot range");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left pot range");
        }
    }
}