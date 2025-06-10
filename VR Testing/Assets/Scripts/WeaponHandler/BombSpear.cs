using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BombSpear : SpearBase
{
    public bool canExplode;
    public bool hasBeenThrown;

    [SerializeField] private float intervalTime;

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            Explodes();
        }
    }

    private void Explodes()
    {
        print("Played explosion particleEffect");
    }
}
