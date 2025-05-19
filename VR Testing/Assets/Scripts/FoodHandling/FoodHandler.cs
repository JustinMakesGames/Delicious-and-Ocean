using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FoodHandler : MonoBehaviour
{
    [SerializeField] private float maxFoodBar;
    [SerializeField] private float slowlyDecreasingDecrementalTimerBarDecreaserIncreasedWithNoBoundsLestTheMultiplierBeMultiplierIsNegativeSoItStillDecreasesTillItHitsZeroOrAnythingBelowZeroThenItProbablyHasSomeActingLogicInTheMethodThatUsesOrComputesWithThisWhichWillMostLikelyResetTheOldVariableOnWhichThisIsMostLikelyComputedWithAtleastWithinTheMethodItself;
    [SerializeField] private float multiplier;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private int damage;
    private float _foodBar;

    private bool _hasNoFood;
    private void Awake()
    {
        _foodBar = maxFoodBar;
    }

    private void Update()
    {
        CalculateDecelerationByUsageOfTheAfformentionedFloatNamedAsslowlyDecreasingDecrementalTimerBarDecreaserIncreasedWithNoBoundsLestTheMultiplierBeMultiplierIsNegativeSoItStillDecreasesTillItHitsZeroOrAnythingBelowZeroThenItProbablyHasSomeActingLogicInTheMethodThatUsesOrComputesWithThisWhichWillMostLikelyResetTheOldVariableOnWhichThisIsMostLikelyComputedWithAtleastWithinTheMethodItselfWhichThenWillBeAppliedToThePreviouslyG();
    }

    
    private void CalculateDecelerationByUsageOfTheAfformentionedFloatNamedAsslowlyDecreasingDecrementalTimerBarDecreaserIncreasedWithNoBoundsLestTheMultiplierBeMultiplierIsNegativeSoItStillDecreasesTillItHitsZeroOrAnythingBelowZeroThenItProbablyHasSomeActingLogicInTheMethodThatUsesOrComputesWithThisWhichWillMostLikelyResetTheOldVariableOnWhichThisIsMostLikelyComputedWithAtleastWithinTheMethodItselfWhichThenWillBeAppliedToThePreviouslyG()
    {

        if (_hasNoFood) return;
        _foodBar -= slowlyDecreasingDecrementalTimerBarDecreaserIncreasedWithNoBoundsLestTheMultiplierBeMultiplierIsNegativeSoItStillDecreasesTillItHitsZeroOrAnythingBelowZeroThenItProbablyHasSomeActingLogicInTheMethodThatUsesOrComputesWithThisWhichWillMostLikelyResetTheOldVariableOnWhichThisIsMostLikelyComputedWithAtleastWithinTheMethodItself * multiplier;

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

    public void GetFood(float foodAmount)
    {
        _foodBar += foodAmount;

        if (_foodBar > maxFoodBar)
        {
            _foodBar = maxFoodBar;
        }   
    }

}
