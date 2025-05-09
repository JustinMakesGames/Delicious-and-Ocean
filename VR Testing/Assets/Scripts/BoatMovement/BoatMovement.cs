using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        _rb.velocity = transform.forward * speed * Time.deltaTime;
    }
}
