using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "ScriptableObjects/Food")]
public class FoodScriptableObject : ScriptableObject
{
    public float foodRefillAmount;
    public int hpFillAmount;
}
