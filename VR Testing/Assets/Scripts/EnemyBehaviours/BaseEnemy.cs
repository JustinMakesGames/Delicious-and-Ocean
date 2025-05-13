using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : ActorBase, IDamagable
{
    public MeshRenderer myRenderer;
    private NavMeshAgent _agent;
    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
    }
    public override void OnDamageTaken(int damage)
    {
        base.OnDamageTaken(damage);
        StartCoroutine(ShowDamageTaken());
    }

    private IEnumerator ShowDamageTaken()
    {
        myRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        myRenderer.material.color = Color.green;
    }


}
