using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] private float speedMultiplier;

    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float decelerationSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private Transform lever;
    [SerializeField] private Transform steeringWheel;
    [SerializeField] private float turnSpeed;

    [Tooltip("This divides the rotation of the wheel so you don't get a turnspeed of 180 degrees")]
    [SerializeField] private float divideRotation;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    
    private void FixedUpdate()
    {
        CalculateVelocity();

        turnSpeed = CalculateTurnSpeed();
        _rb.AddTorque(0, turnSpeed * Time.fixedDeltaTime, 0, ForceMode.Acceleration);
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
}
