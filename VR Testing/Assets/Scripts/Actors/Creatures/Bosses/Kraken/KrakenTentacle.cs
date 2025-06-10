using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenTentacle : ActorChild
{
    [SerializeField] private float _sinkSpeed;
    [SerializeField] private float _despawnTime;

    [SerializeField] private float _sweepAttackInterval;

    private bool _isSinking = false;

    private Vector3 originPos;

    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        originPos = transform.position;
        _animator = GetComponent<Animator>();
        StartCoroutine(SwapStatus());
    }

    private IEnumerator SwapStatus()
    {
        yield return new WaitForSeconds(_sweepAttackInterval);

        if (!_isSinking)
        {
            // Trigger the Animator to play the SweepAttack animation
            _animator.SetBool("SweepAttack", true);
            yield return new WaitForSeconds(3.6f);
            _animator.SetBool("SweepAttack", false);

        }

        StartCoroutine(SwapStatus());
    }

    public override void OnDamageTaken(int damage, DamageType dmgType)
    {
        if (_isSinking) return;
        _parentActor.OnChildDamaged(damage, dmgType);

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

        Kraken kraken = _parentActor as Kraken;
        kraken.OnTentacleDeath(gameObject, originPos);
        Destroy(gameObject);
    }
}
