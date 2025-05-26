using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] private float ballSpeed;

    private void Update()
    {
        transform.Translate(Vector3.forward * ballSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
