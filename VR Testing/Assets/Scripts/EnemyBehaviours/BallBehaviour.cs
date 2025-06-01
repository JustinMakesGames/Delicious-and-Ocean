using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField] private float ballSpeed;
    private int _attackPower;

    private void Start()
    {
        Destroy(gameObject, 3);
    }
    public void MakeAttackStats(int attackPower)
    {
        _attackPower = attackPower;
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * ballSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().TakeDamage(_attackPower);
            Destroy(gameObject);
        }

        if (other.CompareTag("ExplosionTrigger"))
        {
            Destroy(gameObject);
        }
    }
}
