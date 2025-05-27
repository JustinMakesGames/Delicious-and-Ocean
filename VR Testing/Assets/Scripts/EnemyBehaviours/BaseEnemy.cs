using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : ActorBase, IDamagable
{
    public Renderer myRenderer;
    protected override void Awake()
    {
        base.Awake();
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
