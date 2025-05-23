using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class DayTime
{
    public static int CurrentHour { private set; get; }
    public static int CurrentDay { private set; get; }

    public static int HoursTillNight { private set; get; }
    public static int HourDurationInSeconds { private set; get; }

    [Tooltip("Called every (HourDurationInSeconds)")]
    public static UnityEvent<int> OnHourPassed;

    [Tooltip("Called when the hour hits zero")]
    public static UnityEvent<int> OnMorningArrived;

    [Tooltip("Called when the hour hits (hoursTillNight / 2)")]
    public static UnityEvent<int> OnNoonArrived;

    [Tooltip("Called when the hour hits (hoursTillNight)")]
    public static UnityEvent<int> OnNightArrived;


    [Tooltip("Initializes the Daytime, AKA prepares it to be able to be used.")]
    public static void Initialize(int hoursTillNight, int hourDurationInSeconds)
    {
        CurrentHour = 0;
        CurrentDay = 0;

        OnHourPassed = new UnityEvent<int>();
        OnMorningArrived = new UnityEvent<int>();
        OnNoonArrived = new UnityEvent<int>();
        OnNightArrived = new UnityEvent<int>();
    }

    [Tooltip("The time regulator for the entire day and night system. The propagator of all events within the scene")]
    public static IEnumerator TimeRegulator(int hoursTillNight, int hourDurationInSeconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(hourDurationInSeconds);
            CurrentHour++;
            OnHourPassed.Invoke(CurrentHour);

            if (CurrentHour >= hoursTillNight)
            {
                CurrentHour = 0;
                CurrentDay++;
                OnMorningArrived.Invoke(CurrentDay);
            }

            else if (CurrentHour == hoursTillNight / 2)
            {
                OnNoonArrived.Invoke(CurrentDay);
            }

            else if (CurrentHour == hoursTillNight)
            {
                OnNightArrived.Invoke(CurrentDay);
            }
        }
    }

}
