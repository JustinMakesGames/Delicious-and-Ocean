using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[Flags]
public enum CookingType : byte
{
    cuttable = 1,
    cookable = 2,
    fermentable = 4
}

[Serializable]
public struct CookData
{
    public CookingType cookingType;
    public GameObject resultGO;
}

public class Food : MonoBehaviour
{
    [SerializeField] private FoodScriptableObject foodStats;

    public int price;
    private float _refillAmount;
    private int _hpFillAmount;

    private void Awake()
    {
        //Oei
        if (!foodStats) return;
        _refillAmount = foodStats.foodRefillAmount;
        _hpFillAmount = foodStats.hpFillAmount;
        price = foodStats.price;
    }

    #region Cooking

    public CookingType cookingType;
    [SerializeField] private bool _inProcess;
    [SerializeField] private CookData[] _cookData;

    private GameObject _resultGO;

    //Check for the cooker to see whether or not it can cook this food
    private bool CanCookFood(Cooker cooker)
    {
        //In case the cooker is already processing food, return false
        if (cooker._currentlyProcessingFood != null) return false;

        //Circle over all the cookData and see if the cooker can cook this food
        foreach (var CD in _cookData)
        {
            if (CD.cookingType.HasFlag(cooker.cookingType))
            {
                //Set here to prevent checking twice
                _resultGO = CD.resultGO;
                return true;
            }
        }

        //In case of no suitable match found, return
        return false;
    }

    //Called upon the cooker being done(in most cases the start of the day)
    public void OnFoodCooked()
    {
        Instantiate(_resultGO, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    //Called when the food is assigned to the cooker
    public void OnFoodAssigned()
    {
        if(TryGetComponent(out Rigidbody RB))
        {
            RB.isKinematic = true;
        }
    }

    //Temporary check, to be removed
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out Cooker cooker))
        {
            if (CanCookFood(cooker))
            {
                cooker.AssignFood(this);
            }
        }
    }

    #endregion

    public void EatFood(ActivateEventArgs args)
    {
        FoodHandler.Instance.EatFood(_refillAmount);
        Destroy(gameObject);
    }
}
