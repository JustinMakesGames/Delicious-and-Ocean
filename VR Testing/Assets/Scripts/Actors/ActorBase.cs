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

    [Tooltip("This is what kind of drops may be gained out of this actor")]
    [SerializeField] private List<GameObject> _possibleDrops;

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
    public virtual void OnDamageTaken(int damage, DamageType dmgType)
    {
        if (_currentState == ImmunityState.Vulnerable)
        {
            AudioManagement.Instance.PlayAudio("Hit");
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                OnActorDeath();
            }
        }
    }

    //This function is called when the actor dies (Example: When the health of the actor hits 0)
    [ContextMenu("OnActorDeath")]
    protected virtual void OnActorDeath()
    {
        
        SpawnFood();
        Destroy(gameObject);
    }

    private void SpawnFood()
    {
        if (_possibleDrops == null || _possibleDrops.Count == 0) { return; }
        FoodHandler.Instance.GetSomeFood(_possibleDrops, transform);
    }


    [ContextMenu("50% Health damage")]
    public void TestDamage()
    {
        OnDamageTaken(_currentHealth / 2, DamageType.Physical);
    }
}
