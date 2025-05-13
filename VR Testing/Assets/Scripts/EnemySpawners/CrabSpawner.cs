using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CrabSpawner : MonoBehaviour
{
    [SerializeField] private Transform boat;
    [SerializeField] private GameObject crab;
    private Vector3 sideToLook;
    private Bounds _boatBounds;
    private Quaternion rotation;
    private void Awake()
    {
        _boatBounds = boat.GetComponent<Collider>().bounds;
    }

    private void Start()
    {
        SpawnCrabs();
    }
    public void SpawnCrabs()
    {
        Vector3 spawnPosition = ReturnSpawnPosition();
        Quaternion rotation = ReturnRotation(spawnPosition);
        Instantiate(crab, spawnPosition, rotation);
    }

    private Vector3 ReturnSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;
        int spawnIndex = Random.Range(0, 4);

        switch (spawnIndex)
        {
            case 0:
                spawnPosition = new Vector3(Random.Range(_boatBounds.min.x, _boatBounds.max.x), _boatBounds.min.y, _boatBounds.min.z);
                sideToLook = Vector3.forward;
                break;
            case 1:
                spawnPosition = new Vector3(Random.Range(_boatBounds.min.x, _boatBounds.max.x), _boatBounds.min.y, _boatBounds.max.z);
                sideToLook = -Vector3.forward;
                break;
            case 2:
                spawnPosition = new Vector3(_boatBounds.min.x, _boatBounds.min.y, Random.Range(_boatBounds.min.z, _boatBounds.max.z));
                sideToLook = Vector3.right;
                break;
            case 3:
                spawnPosition = new Vector3(_boatBounds.max.x, _boatBounds.min.y, Random.Range(_boatBounds.min.z, _boatBounds.max.z));
                sideToLook = -Vector3.right;
                break;
        }

        return spawnPosition;
    }

    private Quaternion ReturnRotation(Vector3 spawnPosition)
    {
        Quaternion rotation = Quaternion.LookRotation(sideToLook);
        return rotation;
    }
}
