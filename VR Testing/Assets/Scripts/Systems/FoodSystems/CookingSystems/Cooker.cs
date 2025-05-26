using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
    public CookingType cookingType;

    public Food _currentlyProcessingFood { private set; get; }

    [SerializeField] private Transform _processingFoodHolder;

    public void Awake()
    {
        DayTime.OnMorningArrived.AddListener(OnMorningArrived);
    }

    public void AssignFood(Food food)
    {
        _currentlyProcessingFood = food;
        _currentlyProcessingFood.transform.SetParent(_processingFoodHolder);
        _currentlyProcessingFood.transform.position = _processingFoodHolder.position;

        _currentlyProcessingFood.OnFoodAssigned();
    }

    public void OnMorningArrived(int dayCount)
    {
        if(!_currentlyProcessingFood) return;
        _currentlyProcessingFood.OnFoodCooked();
    }
}
