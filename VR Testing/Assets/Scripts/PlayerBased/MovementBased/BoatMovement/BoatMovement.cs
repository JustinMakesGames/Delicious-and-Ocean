using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Filtering;

public enum BoatMovementType
{
    Manual,
    Automatic
}
public class BoatMovement : MonoBehaviour
{
    [SerializeField] private BoatMovementType movementType;

    // [-
    [Header("Shared Variables")]
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float turnSpeed;
    [SerializeField] private float rotationZSpeed;
    [SerializeField] private float minZClamp, maxZClamp;
    private Rigidbody _rb;
    // -]

    // [-
    [Header("Manual Variables")]
    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float decelerationSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private Transform lever;
    [SerializeField] private Transform steeringWheel;
    [Tooltip("This divides the rotation of the wheel so you don't get a turnspeed of 180 degrees")]
    [SerializeField] private float divideRotation;
    // -]

    // [-
    [Header("Automatic Variables")]
    [SerializeField] private float automaticSpeedMultiplier;
    [SerializeField] private Transform[] _wayPoints;
    private Queue<Transform> _wayPointQueue = new Queue<Transform>();
    [SerializeField] private Vector2 _wayPointOffsetWidth;
    [SerializeField] private Vector2 _wayPointOffsetLengthMinMax;
    private Transform _lastPlacedWaypoint;
    [SerializeField] private float tiltIntensity;

    // -]

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        acceleration = 0;
        if (movementType == BoatMovementType.Automatic)
        {
            foreach (Transform point in _wayPoints)
            {
                _wayPointQueue.Enqueue(point);
            }
            _lastPlacedWaypoint = _wayPoints[_wayPoints.Length - 1];
            StartCoroutine(MoveAlongPath());
        }
    }

    #region Manual
    private void FixedUpdate()
    {
        if (movementType == BoatMovementType.Automatic) { return; }
        CalculateVelocity();

        //if (Mathf.Abs(acceleration) < 0.1f) return;
        turnSpeed = CalculateTurnSpeed();

        CalculateZRotation();


        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
    }

    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    private float NormalizeRotation(float angle)
    {
        if (angle > 180f) angle -= 360f;

        return angle;
    }

    private void CalculateVelocity()
    {
        float leverAngle = NormalizeAngle(lever.eulerAngles.z);
        float speed = leverAngle / 10;

        _rb.velocity = transform.forward * acceleration * speedMultiplier * Time.fixedDeltaTime;


        if (leverAngle > -2 && leverAngle < 2)
        {
            acceleration = Mathf.Lerp(acceleration, 0, decelerationSpeed * Time.deltaTime);

            if (acceleration < 0.1)
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

        float turnMultiplier = wheelAngle / divideRotation;

        return turnMultiplier;
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

        transform.eulerAngles = new Vector3(0, currentRotation.y, zAngle);
    }
    #endregion

    #region Automatic
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
        print("Started following waypoints");

        acceleration = speedMultiplier * automaticSpeedMultiplier;
        if (_wayPointQueue.Count == 0 || _rb == null) yield return null;

        while(movementType == BoatMovementType.Automatic && _wayPointQueue.Count > 0)
        {
            var wayPoint = _wayPointQueue.Peek();
            while (Vector3.Distance(transform.position, wayPoint.position) > 1f)
            {
                Vector3 direction = (wayPoint.position - transform.position).normalized;

                _rb.MovePosition(_rb.position + direction * acceleration * Time.deltaTime * speedMultiplier * automaticSpeedMultiplier);

                Quaternion targetRotation = Quaternion.LookRotation(wayPoint.position - transform.position);

                Vector3 cross = Vector3.Cross(transform.forward, direction);
                float tiltAmount = Mathf.Clamp(cross.y, -1f, 1f) * tiltIntensity;

                Quaternion bankRotation = targetRotation * Quaternion.Euler(0, 0, -tiltAmount);

                _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, bankRotation, turnSpeed * Time.deltaTime));

                yield return new WaitForEndOfFrameUnit();
            }


            AssignNewWaypointPosition(wayPoint);
            _wayPointQueue.Enqueue(wayPoint);
            _wayPointQueue.Dequeue();

            yield return new WaitForEndOfFrameUnit();
        }
    }

    #endregion
}
