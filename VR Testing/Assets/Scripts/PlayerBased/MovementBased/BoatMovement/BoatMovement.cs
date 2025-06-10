using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoatMovementType
{
    Manual,
    Automatic,
    disabled
}

public class BoatMovement : MonoBehaviour
{
    [SerializeField] private BoatMovementType movementType;

    [Header("Shared Variables")]
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float rotationZSpeed;
    [SerializeField] private float minZClamp, maxZClamp;

    [Header("Manual Variables")]
    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float decelerationSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private Transform lever;
    [SerializeField] private Transform steeringWheel;
    [Tooltip("This divides the rotation of the wheel so you don't get a turnspeed of 180 degrees")]
    [SerializeField] private float divideRotation;

    [Header("Automatic Variables")]
    [SerializeField] private float _automaticTurnSpeed;
    [SerializeField] private float automaticSpeedMultiplier;
    private Queue<Transform> _wayPointQueue = new Queue<Transform>();
    [SerializeField] private Vector2 _wayPointOffsetWidth;
    [SerializeField] private Vector2 _wayPointOffsetLengthMinMax;
    private Transform _lastPlacedWaypoint;
    [SerializeField] private float tiltIntensity;
    [SerializeField] private int _wayPointCount = 2; //Atleast need 2 otherwise it no workie

    public static BoatMovement Instance;

    private void Start()
    {
        Instance = this;
        TimeEventManager.Instance.OnDayEnd.AddListener(InitBoat);
        InitBoat(0);
    }

    public void InitBoat(int currentDay)
    {
        print("has played" + currentDay);
        acceleration = 0;

        if (ShouldSwitch(currentDay))
        {
            movementType = BoatMovementType.Automatic;
        }
        else
        {
            movementType = BoatMovementType.Manual;
        }

        if (movementType == BoatMovementType.Automatic)
        {
            CreateNewWaypoints(_wayPointCount);
            StartCoroutine(MoveAlongPath());
        }
    }

    private bool ShouldSwitch(int day)
    {
        return day == 1 || day == 3;
    }
    #region Manual
    private void FixedUpdate()
    {
        if (movementType == BoatMovementType.Automatic) return;

        CalculateVelocity();
        turnSpeed = CalculateTurnSpeed();
        CalculateZRotation();

        transform.Translate(Vector3.forward * acceleration * speedMultiplier * Time.fixedDeltaTime, Space.Self);
        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime, Space.Self);
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    private void CalculateVelocity()
    {
        float leverAngle = NormalizeAngle(lever.eulerAngles.z);
        float speed = leverAngle / 10;

        if (leverAngle > -2 && leverAngle < 2)
        {
            acceleration = Mathf.Lerp(acceleration, 0, decelerationSpeed * Time.deltaTime);
            if (acceleration < 0.1f)
            {
                acceleration = 0;
            }
            return;
        }

        if (acceleration * speed < 0)
        {
            acceleration = Mathf.Lerp(acceleration, speed, decelerationSpeed * Time.deltaTime);
            return;
        }

        if (Mathf.Abs(acceleration) < Mathf.Abs(speed))
        {
            acceleration = Mathf.Lerp(acceleration, speed, accelerationSpeed * Time.deltaTime);
        }
    }

    private float CalculateTurnSpeed()
    {
        float wheelAngle = NormalizeAngle(steeringWheel.eulerAngles.z);
        return wheelAngle / divideRotation;
    }

    private void CalculateZRotation()
    {
        Vector3 currentRotation = transform.eulerAngles;
        float zAngle = currentRotation.z;

        if (zAngle > 180f) zAngle -= 360f;

        if (Mathf.Abs(turnSpeed) > 1f)
        {
            zAngle = Mathf.Lerp(zAngle, turnSpeed * 2, Time.deltaTime * 2);
        }
        else
        {
            zAngle = Mathf.Lerp(zAngle, 0, Time.deltaTime * 2);
        }

        zAngle = Mathf.Clamp(zAngle, minZClamp, maxZClamp);
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, zAngle));
    }
    #endregion

    #region Automatic

    private void ClearWaypoints()
    {
        foreach (Transform wayPoint in _wayPointQueue)
        {
            Destroy(wayPoint.gameObject);
        }
        _wayPointQueue.Clear();
    }

    private void CreateNewWaypoints(int count)
    {
        ClearWaypoints();
        _lastPlacedWaypoint = transform;
        for (int i = 0; i < count; i++)
        {
            Transform newWayPoint = new GameObject().transform;
            AssignNewWaypointPosition(newWayPoint);
            _wayPointQueue.Enqueue(newWayPoint);
        }

    }
    private void AssignNewWaypointPosition(Transform wayPoint)
    {
        wayPoint.position = _lastPlacedWaypoint.position + new Vector3(
            Random.Range(_wayPointOffsetWidth.x, _wayPointOffsetWidth.y),
            _lastPlacedWaypoint.position.y,
            Random.Range(_wayPointOffsetLengthMinMax.x, _wayPointOffsetLengthMinMax.y)
        );
        _lastPlacedWaypoint = wayPoint;
    }

    private IEnumerator MoveAlongPath()
    {

        acceleration = speedMultiplier * automaticSpeedMultiplier;
        if (_wayPointQueue.Count == 0) yield break;

        while (movementType == BoatMovementType.Automatic && _wayPointQueue.Count > 0)
        {
            var wayPoint = _wayPointQueue.Peek();

            while (Vector3.Distance(transform.position, wayPoint.position) > 1f)
            {
                Vector3 direction = (wayPoint.position - transform.position).normalized;
                transform.position += direction * acceleration * Time.deltaTime * speedMultiplier * automaticSpeedMultiplier;

                Quaternion targetRotation = Quaternion.LookRotation(wayPoint.position - transform.position);

                Vector3 cross = Vector3.Cross(transform.forward, direction);
                float tiltAmount = Mathf.Clamp(cross.y, -1f, 1f) * tiltIntensity;

                Quaternion bankRotation = targetRotation * Quaternion.Euler(0, 0, -tiltAmount);

                // Try direct rotation first to check if rotation changes:
                // transform.rotation = targetRotation;

                // Use Slerp for smooth rotation with banking:
                transform.rotation = Quaternion.Slerp(transform.rotation, bankRotation, _automaticTurnSpeed * Time.deltaTime);

                yield return new WaitForFixedUpdate();
            }

            _wayPointQueue.Dequeue();

            AssignNewWaypointPosition(wayPoint);
            _wayPointQueue.Enqueue(wayPoint);

            yield return new WaitForFixedUpdate();
        }
    }



    #endregion
}