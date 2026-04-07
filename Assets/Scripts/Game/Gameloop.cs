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

    private int getCurrentDay()
    {
        if (EndingData.currentEnding == endings.BadDish)
        {
            return GameData.currentDay - 1;
        } else
        {
            return GameData.currentDay;
        }
    }

    private void IncreaseDay()
    {
        if (EndingData.currentEnding == endings.BadDish)
        {
            EndingData.currentEnding = endings.None;
            GameData.currentDay = getCurrentDay();
        } else
        {
            GameData.currentDay++;
        }

        dayCount.text = "Day " + GameData.currentDay;
    }

    private void GetNewDish()
    {
        currentDish = dishes[getCurrentDay()];
        dishObjectText.text = currentDish.dishName;
        GameData.currentDish = currentDish;
    }

    public void Died()
    {
        Invoke("StartDeathEnding", 0.5f);
    }

    private void StartDeathEnding()
    {
        //GameData.currentDay = 0;
        EndingData.currentEnding = endings.Death;
        SceneManager.LoadScene("GameOver");
    }

    private void TimeChanged(DayName dayName)
    {
        if (dayName == DayName.Night)
        {
            StartDeathEnding();
        }
    }
}
