using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BombSpear : SpearBase
{
    public bool canExplode;
    public bool hasBeenThrown;

    [SerializeField] private GameObject explosion;

    [SerializeField] private float intervalTime;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            ExplodeSpear();
        }
    }

    public override void OnDamageTaken(int damage, DamageType dmgType)
    {
        base.OnDamageTaken(damage, dmgType);
        if(dmgType == DamageType.Fire)
        {
            ExplodeSpear();
        }
    }

    private void ExplodeSpear()
    {
        Instantiate(explosion, transform.position, Quaternion.identity);
    }
}
