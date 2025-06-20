using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeEventManager : MonoBehaviour
{
    public static TimeEventManager Instance;

    [SerializeField] private int _dayDurationInMinutes;
    [SerializeField] private int _minuteDurationInSeconds = 60;

    [SerializeField] private TMP_Text timerText;
    private int _minuteTimer;
    private int _secondTimer;

    public UnityEvent<int> OnDayEnd = new UnityEvent<int>();
    public UnityEvent<int> OnSecondPassed = new UnityEvent<int>();
    public UnityEvent<int> OnMinutePassed = new UnityEvent<int>();

    public int currentDay = 0;
    public int currentMinute = 0;
    public int currentSecond = 0;

    [SerializeField] private bool _debugValues;

    //This is set to false on uneven days, so the bossfights can happen
    public bool continueTimeRegulation = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance.gameObject);
            Instance = this;
        }

        SetNewTimer();
    }

    private void Start()
    {
        StartCoroutine(RegulateTime());
        if (!_debugValues) { return; }
        OnDayEnd.AddListener((day) => DebugPrint($"Day {day} has ended", DebugType.error));
        OnMinutePassed.AddListener((minute) => DebugPrint($"Minute {minute} has passed", DebugType.warning));
        OnSecondPassed.AddListener((second) => DebugPrint($"Second {second} has passed", DebugType.nrml));
    }
    private IEnumerator RegulateTime()
    {
        var curDay = 0;
        var curSecond = 0;
        var curMinute = 0;

        var yes = true;
        while (yes)
        {
            while (continueTimeRegulation)
            {
                //Can be a hardcoded value for the increment, dont whine. its a second, why the hell would you input anything else then a second
                yield return new WaitForSeconds(1f);
                curSecond++;
                currentSecond++;
                OnSecondPassed.Invoke(curSecond);

                if (curSecond >= _minuteDurationInSeconds)
                {
                    curSecond = 0;
                    currentMinute++;
                    curMinute++;
                    OnMinutePassed.Invoke(curMinute);
                }

                if (curMinute >= _dayDurationInMinutes)
                {
                    curMinute = 0;
                    curMinute++;
                    curDay++;
                    currentDay++;

                    OnDayEnd.Invoke(curDay);

                    // Stop time regulation on uneven days, to do the bossfight
                    if (curDay % 2 != 0)
                    {
                        continueTimeRegulation = false;
                        if (_debugValues) DebugPrint($"Time regulation stopped for day {curDay}, BossFight time!", DebugType.warning);
                    }
                }

                if (_secondTimer <= 0)
                {
                    _minuteTimer--;
                    _secondTimer = _minuteDurationInSeconds;
                }

                _secondTimer--;
                
                timerText.text = _minuteTimer.ToString() +":" + _secondTimer.ToString("00");

                

            }
            yield return null;
        }
    }

    public void SetNewTimer()
    {
        currentDay--;
        _minuteTimer = _dayDurationInMinutes - 1;
        _secondTimer = _minuteDurationInSeconds;
        continueTimeRegulation = true;
    }
    #region Debug Methods
    public enum DebugType
    {
        nrml,
        warning,
        error
    }

    public void DebugPrint(string message, DebugType DT)
    {
        if (DT == DebugType.nrml)
        {
            Debug.Log(message);
        }
        if(DT == DebugType.warning)
        {
            Debug.LogWarning(message);
        }
        if (DT == DebugType.error)
        {
            Debug.LogError(message);
        }
    }
    #endregion
}
