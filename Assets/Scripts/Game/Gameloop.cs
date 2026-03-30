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

    private int days;

    void Start()
    {
        IncreaseDay();
        GetNewDish();

        dayCycle.OnTimeOfDayChange += TimeChanged;
        //plrHealth.OnDeath += Died;
    }

    public Dish GetCurrentDish()
    {
        return currentDish;
    }

    private void IncreaseDay()
    {
        days++;
        dayCount.text = "Day " + days;
    }

    private void GetNewDish()
    {
        currentDish = dishes[Random.Range(0, dishes.Length)];
        dishObjectText.text = currentDish.dishName;
    }

    public void Died()
    {
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
