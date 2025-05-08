using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ActorStats")]
public class ActorStats : ScriptableObject
{
    //The maximum health of the actor
    public int maxHealth;

    //The starting state of the actor, whether the actor can or cannot be hurt. 
    public ImmunityState startState;

}
