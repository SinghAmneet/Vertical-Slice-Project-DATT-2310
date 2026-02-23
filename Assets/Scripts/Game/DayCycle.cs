using System.Collections.Generic;
using System.Text;
using System;
using TMPro;
using UnityEngine;

public enum DayName
{
    Dawn,
    Morning,
    Afternoon,
    Dusk,
    Night,
    Midnight,
}

public struct TimeOfDay
{
    public DayName name;
    public int start;
    
    public TimeOfDay(DayName name, int start)
    {
        this.name = name;
        this.start = start;
    }
}

public class DayCycle : MonoBehaviour
{
    public int startingTime = 6;
    private int time;

    private bool cycling;
    public float rate; // how many in game minutes per irl second

    private float accum;
    private TextMeshProUGUI tmp;
    private TextMeshProUGUI tmpDay;
    private StringBuilder timeText = new(5); // string builder for efficiency

    public event Action<DayName> OnTimeOfDayChange;

    public List<TimeOfDay> timesOfDay = new()
    {
        new TimeOfDay(DayName.Dawn, 5),         // dawn: 5am - 6am
        new TimeOfDay(DayName.Morning, 6),      // morning: 6am - 12pm
        new TimeOfDay(DayName.Afternoon, 12),   // afternoon: 12pm - 6pm
        new TimeOfDay(DayName.Dusk, 18),        // dusk: 6pm - 7pm
        new TimeOfDay(DayName.Night, 19),       // night: 7pm - 12am
        new TimeOfDay(DayName.Midnight, 24),    // midnight: 12am - 5am
    };

    private int currentHour;
    private DayName currentTOD;

    // pause or resume the day cycle
    public void UpdateCycle(bool pause)
    {
        cycling = !pause;
    }

    private void Awake()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tmpDay = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        cycling = true;
        SetTime(startingTime);
    }

    public DayName GetTimeOfDay()
    {
        for (int i = 0; i < timesOfDay.Count; i++)
        {
            TimeOfDay tod = timesOfDay[i];
            if (i + 1 == timesOfDay.Count) break;
            TimeOfDay nextTod = timesOfDay[i + 1];

            if (currentHour >= tod.start && currentHour < nextTod.start)
            {
                return tod.name;
            }
        }
        return DayName.Midnight;
    }

    // set the name of current TOD and invoke change event
    private void SetTimeOfDay()
    {
        if (currentHour != GetHour())
        {
            currentHour = GetHour();
            DayName tod = GetTimeOfDay();
            
            // TOD name changed
            if (tod != currentTOD) {
                currentTOD = tod;
                tmpDay.text = tod.ToString();
                OnTimeOfDayChange?.Invoke(tod);
                //Debug.Log("Time of day changed to: " + tod.ToString());
            }

        }
    }

    private int GetHour()
    {
        return Mathf.FloorToInt(time / 60);
    }
    
    // set time of day
    public void SetTime(int newTime)
    {
        time = newTime * 60;
        SetTimeOfDay();
        tmpDay.text = currentTOD.ToString();
        SetFormatTime();
    }

    // format text ui
    private void SetFormatTime()
    {
        timeText.Clear();

        int hour = GetHour();
        int minute = time - (hour * 60);

        // append strings to string builder
        if (hour < 10) timeText.Append("0");
        timeText.Append(hour);
        timeText.Append(":");
        if (minute < 10) timeText.Append("0");
        timeText.Append(minute);

        tmp.text = timeText.ToString();
    }

    private void IncreaseTime()
    {
        accum = 0;
        time++;
        if (GetHour() > 23) time = 0;
        SetTimeOfDay();
    }

    void Update()
    {
        if (!cycling) return;
        accum += rate * Time.deltaTime;
        
        if (accum > 1)
        {
            IncreaseTime();
            SetFormatTime();
        }
    }
}
