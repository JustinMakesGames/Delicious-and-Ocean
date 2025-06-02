using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearBase : WeaponActor
{
    private bool _stickToTarget = false;

    //Test function to test whether or not damaging an actor works
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
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
}
