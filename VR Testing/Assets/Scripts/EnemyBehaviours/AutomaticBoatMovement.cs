using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AutomaticBoatMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody _rb;

    private void Update()
    {
        Vector3 velocity = Vector3.forward * speed * Time.deltaTime;

        velocity.y = 0;
        _rb.velocity = velocity;

    }

    private void CalculateShaking()
    {

    }
}
