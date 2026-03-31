using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text tutorialText;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private Health mushroomHealth;
    [SerializeField] private TutorialFinishPot tutorialPot;

    [Header("Step Requirements")]
    [SerializeField] private float targetMoveX = 20f;
    [SerializeField] private int requiredPickupCount = 2;

    private int step = 0;

    private bool mushroomDead = false;
    private bool tutorialFinished = false;

    private void Awake()
    {
        if (mushroomHealth != null)
        {
            mushroomHealth.OnDeath += OnMushroomDeath;
        }
    }

    private void Start()
    {
        ShowStep();
    }

    private void Update()
    {
        if (tutorialFinished) return;

        CheckProgress();
    }

    private void CheckProgress()
    {
        switch (step)
        {
            // STEP 0: Move until player reaches x = 30
            case 0:
                if (player != null && player.position.x >= targetMoveX)
                {
                    NextStep();
                }
                break;

            // STEP 1: Pick up 2 items
            case 1:
                if (playerInventory != null && playerInventory.GetItems().Count >= requiredPickupCount)
                {
                    NextStep();
                }
                break;

            // STEP 2: Drop all picked-up items
            case 2:
                if (playerInventory != null && playerInventory.GetItems().Count == 0)
                {
                    NextStep();
                }
                break;

            // STEP 3: Slay mushroom
            case 3:
                if (mushroomDead)
                {
                    NextStep();
                }
                break;

            // STEP 4: Pot instruction stays until pot interaction hides text
            case 4:
                // Wait here. Pot script will hide text and mark tutorial complete.
                break;
        }
    }

    private void OnMushroomDeath()
    {
        mushroomDead = true;
    }

    private void NextStep()
    {
        step++;
        ShowStep();
    }

    private void ShowStep()
    {
        if (tutorialText == null) return;

        tutorialText.gameObject.SetActive(true);

        switch (step)
        {
            case 0:
                tutorialText.text = "Use A, W, S, and D to move Kenny";
                break;

            case 1:
                tutorialText.text = "Use Right click to pick up an ingredient off the ground";
                break;

            case 2:
                tutorialText.text = "Select an inventory slot with 1 to 6 on your keyboard. Since you only have two items in your inventory, select 1 or 2 and then press Q to drop the item";
                break;

            case 3:
                tutorialText.text = "Use Left click to attack and slay the mushroom.";
                break;

            case 4:
                tutorialText.text = "Go to the cooking pot and press F to interact.";
                break;

            default:
                tutorialText.text = "";
                break;
        }
    }

    public void HideTutorialText()
    {
        if (tutorialText != null)
        {
            tutorialText.text = "";
            tutorialText.gameObject.SetActive(false);
        }

        tutorialFinished = true;
    }

    public int GetCurrentStep()
    {
        return step;
    }
}