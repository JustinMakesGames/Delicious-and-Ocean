using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WhirlPool : MonoBehaviour
{
    [SerializeField] private GameObject fish;
    [SerializeField] private int fishAmount;

    private List<Transform> _fishList = new List<Transform>();
    private Bounds _bounds;


    private void Awake()
    {
        _bounds = GetComponent<Collider>().bounds;
    }

    private void Start()
    {
        SpawnFish();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            Transform fish = CalculateClosestFish(other.transform);
            _fishList.Remove(fish);
            fish.GetComponent<ActorBase>().OnDamageTaken(other.GetComponent<WeaponActor>().WeaponDamage(), DamageType.Physical);
        }
    }

    private void SpawnFish()
    {
        for (int i = 0; i < fishAmount; i++)
        {
            Vector3 position = ReturnSpawnPosition();
            GameObject fishClone = Instantiate(fish, position, Quaternion.identity, transform);
            _fishList.Add(fishClone.transform);
        }
    }

    private Vector3 ReturnSpawnPosition()
    {
        Vector3 spawnPosiion = new Vector3(Random.Range(_bounds.min.x, _bounds.max.x), _bounds.max.y, Random.Range(_bounds.min.z, _bounds.max.z));

        return spawnPosiion;
    }

    private Transform CalculateClosestFish(Transform weapon)
    {
        Transform fish = null;
        float smallestDistance = Mathf.Infinity;

        for (int i = 0; i < _fishList.Count; i++)
        {
            float distance = Vector3.Distance(weapon.position, _fishList[i].position);

            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                fish = _fishList[i];
            }
        }
        return fish;
    }
}
