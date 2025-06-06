using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellSystem : MonoBehaviour
{
    public PlayerStats playerStats;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            playerStats.SellItem(other.GetComponent<Food>().price);
            Destroy(other.gameObject);
        }
    }
}
