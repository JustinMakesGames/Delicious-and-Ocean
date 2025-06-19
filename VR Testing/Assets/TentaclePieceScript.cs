using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentaclePieceScript : ActorChild
{
    [SerializeField] private SphereCollider _sphereCollider;

    [SerializeField] private float _damageInterval = 1f;

    [SerializeField] private bool _canHit = true;

    [SerializeField] private int tentacleDamage;
    public void Update()
    {
        if(!_canHit) { return; }
        if (Vector3.Distance(PlayerStats.PlayerPos.position, transform.position) <= _sphereCollider.radius)
        {
            PlayerStats.Instance.OnDamageTaken(tentacleDamage, DamageType.Physical);
            _canHit = false;
            StartCoroutine(AttackCooldown());
        }
    }
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(_damageInterval);
        _canHit = true;
    }
    public override void OnDamageTaken(int damage, DamageType dmgType)
    {
        _parentActor.OnChildDamaged(damage, dmgType);
    }
}
