using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ActorStats")]
public class ActorStats : ScriptableObject
{
    [Tooltip("The maximum health of the actor, the current health of the actor cannot exceed this value")]
    public int maxHealth;

    [Tooltip("The starting state of the actor, whether the actor can or cannot be hurt. ")]
    public ImmunityState startState;

}
