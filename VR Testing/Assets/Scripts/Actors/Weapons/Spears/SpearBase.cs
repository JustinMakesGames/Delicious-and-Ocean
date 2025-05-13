using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearBase : ActorBase
{
    [Header("Spear Stats")]

    [Tooltip("The damage of the spear")]
    [SerializeField] private int _spearDamage;

    protected override void Init()
    {
        base.Init();
        if (_actorStatsSO)
        {
            _spearDamage = _actorStatsSO.startDamage;
        }
        else
        {
            print("No actorStatsSO assigned to the spear, so no damage");
        }
    }
    //Test function to test whether or not damaging an actor works
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.OnDamageTaken(_spearDamage);
        }
    }
}
