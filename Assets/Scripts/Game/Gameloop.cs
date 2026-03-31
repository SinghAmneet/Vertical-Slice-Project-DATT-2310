using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Gameloop : MonoBehaviour
{
    public DayCycle dayCycle;
    public TextMeshProUGUI dayCount;

    public TextMeshProUGUI dishObjectText;

    public Health plrHealth;

    public Dish[] dishes;
    public List<Dish> finishedDishes;
    private Dish currentDish;

    public GameObject transitionPanel;

    private void Awake()
    {
        transitionPanel.SetActive(true);
    }

    void Start()
    {

        if (!GameData.hasDoneTutorial && GameData.currentDay == 0)
        {
            SceneManager.LoadScene("TutorialScene");
            return;
        }

        if (!GameData.loadedDialogue)
        {
            GameData.loadedDialogue = true;
            LoadDialogueScene();
            return;
        }
        transitionPanel.SetActive(false);
        GetNewDish();
        IncreaseDay();

        dayCycle.OnTimeOfDayChange += TimeChanged;
    }

    private void LoadDialogueScene()
    {
        switch (GameData.currentDay)
        {
            case 1:
                SceneManager.LoadScene("DialogueR1");
                break;
            case 2:
                SceneManager.LoadScene("DialogueR2");
                break;
            case 3:
                SceneManager.LoadScene("DialogueR3");
                break;
        }
    }

    public Dish GetCurrentDish()
    {
        return currentDish;
    }

    private void IncreaseDay()
    {
        if (EndingData.currentEnding != endings.BadDish) { GameData.currentDay++; } else
        {
            GameData.currentDay = Mathf.Max(GameData.currentDay - 1, 0);
        }

        dayCount.text = "Day " + GameData.currentDay;
    }

    private void GetNewDish()
    {
        int day = GameData.currentDay;
        if (day >= 3) day = 2;
        currentDish = dishes[day];
        dishObjectText.text = currentDish.dishName;
    }

    public void Died()
    {
        Invoke("StartDeathEnding", 0.5f);
    }

    private void StartDeathEnding()
    {
        GameData.currentDay = 0;
        EndingData.currentEnding = endings.Death;
        SceneManager.LoadScene("GameOver");
    }

    private void TimeChanged(DayName dayName)
    {
        if (dayName == DayName.Night)
        {
            Debug.Log("Timer is up");
        }
    }
}
