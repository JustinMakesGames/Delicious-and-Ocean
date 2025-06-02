using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackHandPosition : MonoBehaviour
{
    public Vector3 Velocity { get; private set;}
    private Vector3 _lastPosition;


    void Start()
    {
        _lastPosition = transform.position;
    }


    private void LateUpdate()
    {
        Velocity = (transform.position - _lastPosition) / Time.deltaTime;
        _lastPosition = transform.position;
    }
}
