using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : ActorBase
{
    [Header("Firing Settings")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _shootingInterval;
    [SerializeField] private bool _canShoot = true;

    [Header("Cannon Ball Settings")]
    [SerializeField] private float _cannonBallSpeed;
    [SerializeField] private GameObject _cannonBallPrefab;

    public override void OnDamageTaken(int damage, DamageType dmgType)
    {
        print(dmgType);
        if (dmgType.HasFlag(DamageType.Fire))
        {
            if (CanShoot())
            {
                ShootCannon();
            }
        }
    }

    private bool CanShoot()
    {
        return _canShoot;
    }

    private void ShootCannon()
    {
        print("Shooting");
        _canShoot = false;
        GameObject cannonBall = Instantiate(_cannonBallPrefab, _firePoint.position, _firePoint.rotation);
        Rigidbody rb = cannonBall.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = _firePoint.forward * _cannonBallSpeed;
        }
        StartCoroutine(ResetShooting());
    }

    private IEnumerator ResetShooting()
    {
        yield return new WaitForSeconds(_shootingInterval);
        _canShoot = true;
    }

   
}
