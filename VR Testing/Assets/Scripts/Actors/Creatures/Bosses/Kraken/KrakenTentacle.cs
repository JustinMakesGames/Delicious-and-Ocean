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

    [SerializeField] private Vector3 _attackBoxSize;
    [SerializeField] private Vector3 _attackBoxOffset;

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
            _animator.SetBool("SweepAttack", true);

            yield return new WaitForSeconds(1f);
            DamagePlayersInAttackBox();

            yield return new WaitForSeconds(2.6f);
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
    private void DamagePlayersInAttackBox()
    {
        Vector3 worldCenter = transform.position + transform.rotation * _attackBoxOffset;
        Vector3 halfExtents = _attackBoxSize * 0.5f;
        Collider[] hits = Physics.OverlapBox(worldCenter, halfExtents, transform.rotation);


        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<PlayerStats>(out PlayerStats player))
            {
                if (player != null)
                {
                    player.GetComponent<IDamagable>().OnDamageTaken(_actorStatsSO.startDamage, DamageType.Physical);
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position + transform.rotation * _attackBoxOffset, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;
        Gizmos.DrawWireCube(Vector3.zero, _attackBoxSize);
    }

}
