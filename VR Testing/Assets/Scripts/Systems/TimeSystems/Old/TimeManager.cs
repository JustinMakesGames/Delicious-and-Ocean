using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Tooltip("Make sure the time is set to 24 hours, otherwise there will be no noon-event call")]
    [SerializeField] private int _hoursTillNight = 12;
    
    //Made initializers cuz it'd suck if this shit would spam my console-thingy full of: -
    // -"hour has passed, pretty cool currently at (Insert amount of hourspassed)
    [SerializeField] private int _hourDurationInSeconds = 30;

    [SerializeField] private bool _debugPrints;
    private void Awake()
    {
        DayTime.Initialize(_hoursTillNight, _hourDurationInSeconds);
        StartCoroutine(DayTime.TimeRegulator(_hoursTillNight, _hourDurationInSeconds));

        if (!_debugPrints) return;
        DayTime.OnHourPassed.AddListener(OnHourPassed);
        DayTime.OnMorningArrived.AddListener(OnMorningArrived);
    }

    private void OnHourPassed(int hourPassed)
    {
        print($"Hour has passed, pretty cool, currently at {hourPassed}");
    }
    private void OnMorningArrived(int dayPassed)
    {
        print($"Another morning has arised, pretty cool, currently at day {dayPassed}");
    }
}
