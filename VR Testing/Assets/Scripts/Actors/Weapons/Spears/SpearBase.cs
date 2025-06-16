using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpearBase : WeaponActor
{
    public bool isHolding;
    private bool _stickToTarget = false;

    //Test function to test whether or not damaging an actor works
    public override void OnTriggerEnter(Collider collision)
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
