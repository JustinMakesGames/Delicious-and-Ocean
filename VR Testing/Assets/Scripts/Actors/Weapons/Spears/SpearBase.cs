using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpearBase : WeaponActor
{
    public bool isHolding;
    private bool _stickToTarget = false;

    //Whether or not this spear is oiled up, which makes it be able to be lit on fire
    [SerializeField] private bool _isOiledUp;

    [SerializeField] private Transform _fireHolder;
    [SerializeField] private GameObject _fire;
    [SerializeField] private int fireDamage;

    //Test function to test whether or not damaging an actor works
    protected override void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            if(collision.TryGetComponent(out WeaponActor weaponActor))
            {
                if(weaponActor.WeaponDamageType() == DamageType.Fire && _isOiledUp)
                {
                    print("Spear is on fire, cannot stick to target");
                    _stickToTarget = false;
                }
                else
                {
                    _stickToTarget = true;
                }
                return;
            }
            damagable.OnDamageTaken(WeaponDamage());
            if (_stickToTarget)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                transform.SetParent(collision.transform);
                return;
            }
            print("Hit " + collision.name + " with spear, dealing " + WeaponDamage() + " damage");
            Destroy(gameObject);
        }
    }
    public override void OnDamageTaken(int damage, DamageType dmgType)
    {
        print(dmgType);
        base.OnDamageTaken(damage, dmgType);
        if(dmgType == DamageType.Fire && _isOiledUp)
        {
            if (_fireHolder.childCount == 0)
            {
                GameObject fire = Instantiate(_fire, _fireHolder);
                fire.transform.localScale = Vector3.one * 0.2f; 
                _weaponDamage += fireDamage;
            }
        }

        if(dmgType == DamageType.Oil)
        {
            _isOiledUp = true;
            print("Spear is now oiled up, it can be lit on fire");
        }

    }
    private void LateUpdate()
    {
        if (isHolding)
        {
            transform.rotation = Quaternion.Euler(90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }
    public void SetRotationRight(SelectEnterEventArgs args)
    {
        isHolding = true;
        transform.rotation = Quaternion.Euler(90, 180, 0);
    }

    public void SetRotationOff(SelectExitEventArgs args)
    {
        isHolding = false;
    }
}
