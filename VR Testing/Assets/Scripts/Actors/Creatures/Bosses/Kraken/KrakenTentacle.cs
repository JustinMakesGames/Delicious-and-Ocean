using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenTentacle : ActorChild
{
    [SerializeField] private float _sinkSpeed;
    [SerializeField] private float _despawnTime;

    private bool _isSinking = false;

    private Vector3 originPos;

    protected override void Awake()
    {
        base.Awake();
        originPos = transform.position;
    }
    public override void OnDamageTaken(int damage, DamageType dmgType)
    {
        if (_isSinking) return;
        _parentActor.OnDamageTaken(damage, dmgType);

        if (_currentState == ImmunityState.Vulnerable)
        {
            _currentHealth -= damage;
            if (_currentHealth <= 0)
            {
                OnActorDeath();
            }
        }
    }
    [ContextMenu("OnTentacleDeath")]
    protected override void OnActorDeath()
    {
        print("Real");
        StartCoroutine(Sink());
    }

    private IEnumerator Sink()
    {
        _isSinking = true;
        float elapsedTime = 0f;
        while (elapsedTime < _despawnTime)
        {
            transform.position += Vector3.down * _sinkSpeed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        print(_parentActor.name + " tentacle has sunk and will be destroyed now.");
        Kraken kraken = _parentActor as Kraken;
        kraken.OnTentacleDeath(gameObject, originPos);
        Destroy(gameObject);
    }
}