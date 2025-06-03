using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeEventManager : MonoBehaviour
{
    public static TimeEventManager Instance { get; private set; }

    [SerializeField] private int _dayDurationInMinutes;
    [SerializeField] private int _minuteDurationInSeconds = 60;

    public UnityEvent<int> OnDayEnd = new UnityEvent<int>();
    public UnityEvent<int> OnSecondPassed = new UnityEvent<int>();
    public UnityEvent<int> OnMinutePassed = new UnityEvent<int>();

    [SerializeField] private bool _debugValues;
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
        while (true)
        {
            //Can be a hardcoded value for the increment, dont whine. its a second, why the hell would you input anything else then a second
            yield return new WaitForSeconds(1f);
            curSecond++;

            OnSecondPassed.Invoke(curSecond);
            if (curSecond >= _minuteDurationInSeconds)
            {
                curSecond = 0;
                curMinute++;
                OnMinutePassed.Invoke(curMinute);
            }
            if (curMinute >= _dayDurationInMinutes)
            {
                curMinute = 0;
                curDay++;
                OnDayEnd.Invoke(curDay);
            }

        }
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
