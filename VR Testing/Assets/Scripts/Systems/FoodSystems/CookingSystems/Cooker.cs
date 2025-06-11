using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooker : MonoBehaviour
{
    public CookingType cookingType;

    public Food _currentlyProcessingFood { private set; get; }

    [SerializeField] private Transform _processingFoodHolder;

    private IEnumerator CookFood()
    {
        yield return new WaitForSeconds(10);
        OnMorningArrived();
    }
    public void AssignFood(Food food)
    {
        _currentlyProcessingFood = food;
        _currentlyProcessingFood.transform.SetParent(_processingFoodHolder);
        _currentlyProcessingFood.transform.position = _processingFoodHolder.position;

        StartCoroutine(CookFood());
        _currentlyProcessingFood.OnFoodAssigned();
    }

    public void OnMorningArrived()
    {
        if(!_currentlyProcessingFood) return;
        _currentlyProcessingFood.OnFoodCooked();
    }
}
