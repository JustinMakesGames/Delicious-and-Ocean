using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActorBase : MonoBehaviour, IDamagable
{
    [Header("Actor Base Stats")]

    [Tooltip("This acts as the base stats of this actor")]
    [SerializeField] protected ActorStats _actorStatsSO;

    [Tooltip("This is the current health of this actor")]
    [SerializeField] protected int _currentHealth;

    [Tooltip("The Immunity state of the actor, whether it can, or cannot be hurt")]
    [SerializeField] protected ImmunityState _currentState;

    //Standard unity awake
    protected virtual void Awake()
    {
        Init();
    }

    //Initialize the stats or other variables within the derived/base actor
    protected virtual void Init()
    {
        //Checks whether or not an ActorStatsSO is assigned to this actor
        if (_actorStatsSO != null)
        {
            _currentHealth = _actorStatsSO.maxHealth;
            _currentState = _actorStatsSO.startState;
        }

    }

    //This function is called when the actor takes damage
    public virtual void OnDamageTaken(int damage)
    {
        if(_currentState == ImmunityState.Vulnerable)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                OnActorDeath();
            }
        }
    }

    //This function is called when the actor dies (Example: When the health of the actor hits 0)
    protected virtual void OnActorDeath()
    {
        //Example logic, 
        Destroy(gameObject);
    }
}
