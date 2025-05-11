using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] private float speedMultiplier;

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

        print("AngularVelocity: " + _rb.angularVelocity);
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
        float speed = leverAngle / 90;

        _rb.velocity = transform.forward * speed * speedMultiplier * Time.fixedDeltaTime;

    }

    private float CalculateTurnSpeed()
    {
        float wheelAngle = NormalizeAngle(steeringWheel.eulerAngles.z);

        float turnMultiplier = wheelAngle / divideRotation;

        return turnMultiplier;
    }
}
