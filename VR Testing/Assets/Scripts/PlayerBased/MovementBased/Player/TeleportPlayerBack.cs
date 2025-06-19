using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerBack : MonoBehaviour
{
    private Transform playerPosition;
    private void Awake()
    {
        playerPosition = GameObject.FindGameObjectWithTag("PlayerPosition").transform;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = playerPosition.position;
        }
    }
}
