using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentaclePieceScript : WeaponActor
{
    [SerializeField] private SphereCollider _sphereCollider;

    [SerializeField] private float _damageInterval = 1f;

    [SerializeField] private bool _canHit = true;
    public void Update()
    {
        if(!_canHit) { return; }
        if (Vector3.Distance(PlayerStats.PlayerPos.position, transform.position) <= _sphereCollider.radius)
        {
            PlayerStats.Instance.OnDamageTaken(WeaponDamage(), DamageType.Physical);
            _canHit = false;
            print("Hit Player with Tentacle Piece, dealing " + WeaponDamage() + " damage");
            StartCoroutine(AttackCooldown());
        }
    }
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_damageInterval);
        _canHit = true;
    }
}
