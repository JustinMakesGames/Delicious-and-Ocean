using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponActor : ActorBase
{
    public int WeaponDamage() { return _weaponDamage; }
    [SerializeField] protected int _weaponDamage;

    [SerializeField]private bool _oneTimeUse = false;

    [SerializeField]private DamageType _damageType;
    public DamageType WeaponDamageType() { return _damageType; }
    protected override void Init()
    {
        base.Init();
        if (_actorStatsSO)
        {
            _weaponDamage = _actorStatsSO.startDamage;
            _damageType = _actorStatsSO.damageType;
        }
    }

    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.OnDamageTaken(WeaponDamage(), _damageType);

            print(gameObject.name + "  Hit " + collision.name + " with weapon, dealing " + WeaponDamage() + " damage");
            if(_oneTimeUse)
            {
                Destroy(gameObject);
            }
        }
    }
}
