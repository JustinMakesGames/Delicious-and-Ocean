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
        float realRadius = _sphereCollider.radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);

        float distance = Vector3.Distance(PlayerStats.PlayerPos.position, transform.position);
        print(distance);
        if (distance <= realRadius)
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
