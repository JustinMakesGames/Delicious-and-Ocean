using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private int price;
    [SerializeField] private BarrelHandler barrelHandler;
    [SerializeField] private PlayerStats playerStats;

    public void BuyItem()
    {
        if (playerStats.coins < price) return;

        AudioManagement.Instance.PlayAudio("BuySound");
        playerStats.coins -= price;

        barrelHandler.GetSpear();
    }
}
