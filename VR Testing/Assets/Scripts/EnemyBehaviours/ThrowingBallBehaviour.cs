using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingBallBehaviour : MonoBehaviour
{
    [SerializeField] private float launchHeight;
    [SerializeField] private Transform boat;
    [SerializeField] private float speedMultiplier;

    private Vector3 _targetPlace;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _targetPlace = GetRandomPosition();
        LaunchBall();
    }

    private Vector3 GetRandomPosition()
    {
        Bounds bounds = boat.GetComponent<Collider>().bounds;

        float xAxis = Random.Range(bounds.min.x, bounds.max.x);
        float zAxis = Random.Range(bounds.min.z, bounds.max.z);

        Vector3 randomPosition = new Vector3(xAxis, bounds.max.y, zAxis);

        return randomPosition;
    }

    private void LaunchBall()
    {
        float scaledGravity = Physics.gravity.y / (speedMultiplier * speedMultiplier);
        Vector3 start = transform.position;
        Vector3 displacement = _targetPlace - start;

        Vector3 displacementXZ = new Vector3(displacement.x, 0, displacement.z);
        float time = (Mathf.Sqrt(-2 * launchHeight / scaledGravity) +
                     Mathf.Sqrt(2 * (displacement.y - launchHeight) / scaledGravity)) * speedMultiplier;

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * scaledGravity * launchHeight);
        Vector3 velocityXZ = displacementXZ / time;

        _rb.velocity = velocityXZ + velocityY;
    }
}
