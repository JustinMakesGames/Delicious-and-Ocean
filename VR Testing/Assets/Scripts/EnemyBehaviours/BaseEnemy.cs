using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour, IDamagable
{
    public MeshRenderer myRenderer;
    public void OnDamageTaken(int damage)
    {
        StartCoroutine(ShowDamageTaken());
    }

    private IEnumerator ShowDamageTaken()
    {
        myRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        myRenderer.material.color = Color.green;
    }
}
