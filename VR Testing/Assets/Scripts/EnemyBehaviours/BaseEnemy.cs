using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : ActorBase, IDamagable
{
    public Renderer myRenderer;
    protected int attackPower;
    Color color;
    protected override void Awake()
    {
        base.Awake();
        color = myRenderer.material.color;
        attackPower = _actorStatsSO.startDamage;
    }
    public override void OnDamageTaken(int damage, DamageType dmgType)
    {
        base.OnDamageTaken(damage, dmgType);
        StartCoroutine(ShowDamageTaken());
    }
    private IEnumerator ShowDamageTaken()
    {
        myRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        myRenderer.material.color = color;
    }
}
