using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Food : MonoBehaviour
{
    [SerializeField] private FoodScriptableObject foodStats;

    private float _refillAmount;
    private int _hpFillAmount;

    private void Awake()
    {
        _refillAmount = foodStats.foodRefillAmount;
        _hpFillAmount = foodStats.hpFillAmount;
    }

    public void EatFood(ActivateEventArgs args)
    {
        FoodHandler.Instance.EatFood(_refillAmount);
        Destroy(gameObject);
    }
}
