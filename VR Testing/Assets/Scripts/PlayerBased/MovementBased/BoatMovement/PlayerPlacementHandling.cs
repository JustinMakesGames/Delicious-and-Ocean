using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlacementHandling : MonoBehaviour
{
    [SerializeField] private Transform boat;
    private bool _isPlayerOnBoat;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_isPlayerOnBoat)
        {
            print(gameObject.name +     "   It happened");
            other.transform.parent = boat;
            _isPlayerOnBoat = true;
        }
    }
}
