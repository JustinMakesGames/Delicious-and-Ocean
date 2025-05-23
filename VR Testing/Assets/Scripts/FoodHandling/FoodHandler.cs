using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodHandler : MonoBehaviour
{
    public static FoodHandler Instance;

    [SerializeField] private float maxFoodBar;
    [SerializeField] private float decreasingFoodDecremental;
    [SerializeField] private float multiplier;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private int damage;

    [SerializeField] private Transform foodLocation;
    [SerializeField] private GameObject foodCanvas;

    [SerializeField] private Slider foodBarSlider;
    private Bounds _foodLocationBounds;
    private float _foodBar;

    private bool _hasNoFood;
    private void Awake()
    {
        _foodBar = maxFoodBar;
        foodBarSlider.maxValue = maxFoodBar;
        foodBarSlider.value = maxFoodBar;
        _foodLocationBounds = foodLocation.GetComponent<Collider>().bounds;

        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        CalculateFoodDecremental();
    }

    
    private void CalculateFoodDecremental()
    {

        if (_hasNoFood) return;
        _foodBar -= decreasingFoodDecremental * multiplier;
        foodBarSlider.value = _foodBar;

        if (_foodBar <= 0)
        {
            _hasNoFood = true;
            StartCoroutine(RemoveHP());
        }
    }

    private IEnumerator RemoveHP()
    {
        while (_hasNoFood)
        {
            yield return new WaitForSeconds(2);
            playerStats.TakeDamage(damage);
        }
    }

    public void EatFood(float foodAmount)
    {
        _foodBar += foodAmount;

        _hasNoFood = false;
        if (_foodBar > maxFoodBar)
        {
            _foodBar = maxFoodBar;
        }   
    }

    public void GetSomeFood(List<GameObject> possibleFood, Transform enemy)
    {
        int foodAmount = 2;

        List<GameObject> foodOutcome = new List<GameObject>();
        for (int i = 0; i < foodAmount; i++)
        {
            int randomFood = Random.Range(0, possibleFood.Count);
            foodOutcome.Add(possibleFood[randomFood]);
        }

        GameObject foodCanvasClone = Instantiate(foodCanvas, enemy.position, Quaternion.identity);

        TMP_Text text = foodCanvasClone.GetComponentInChildren<TMP_Text>();

        for (int i = 0; i < foodOutcome.Count; i++)
        {
            text.text += "You got a " + foodOutcome[i].name + "\n";
            SpawnFoodInTheRoom(foodOutcome[i]);
        }
        

        
    }

    private void SpawnFoodInTheRoom(GameObject food)
    {
        Vector3 randomPosition = new Vector3(Random.Range(_foodLocationBounds.min.x, _foodLocationBounds.max.x), _foodLocationBounds.center.y, _foodLocationBounds.center.z);

        Instantiate(food, randomPosition, Quaternion.identity);
    }
}
