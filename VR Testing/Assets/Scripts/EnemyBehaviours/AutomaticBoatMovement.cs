using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class AutomaticBoatMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationAmount;

    private float _angle;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {

        MoveForwards();
        CalculateShaking();

    }

    private void MoveForwards()
    {
        Vector3 velocity = Vector3.forward * speed * Time.deltaTime;

        velocity.y = 0;
        _rb.velocity = velocity;
    }

    private void CalculateShaking()
    {
        _angle += Time.deltaTime * rotationSpeed;
        float zRotation = Mathf.Sin(_angle) * rotationAmount;

        transform.localRotation = Quaternion.Euler(0, 0, zRotation);
    }
}
