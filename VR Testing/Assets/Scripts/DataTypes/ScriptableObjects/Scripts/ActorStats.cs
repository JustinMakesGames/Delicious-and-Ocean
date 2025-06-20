using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Actors/ActorStats")]
public class ActorStats : ScriptableObject
{
    [Tooltip("The maximum health of the actor, the current health of the actor cannot exceed this value. ")]
    public int maxHealth;

    [Tooltip("The damage the actor may, or may not use. ")]
    public int startDamage;

    [Tooltip("The starting state of the actor, whether the actor can or cannot be hurt. ")]
    public ImmunityState startState;

    public DamageType damageType;

    //Due to timeshortage, putting it in here
    public int fireDamage;
}
