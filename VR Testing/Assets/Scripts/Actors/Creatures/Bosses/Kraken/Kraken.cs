using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Kraken : ActorParent
{
    [Header("Tentacle spawning settings")]
    [SerializeField] private GameObject _tentacleGO;

    private List<GameObject> _activeTentacles = new List<GameObject>();
    private List<Vector3> _usedPositions = new List<Vector3>();

    [SerializeField] private float _spawnTentacleAttempt;

    [SerializeField] private int _maxTentacles = 3;

    public List<Transform> tentacleSpawnPositions;
    public Transform boatTransform;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(TentacleSpawner());
    }

    public void OnTentacleDeath(GameObject tentacle, Vector3 originalPos)
    {
        _activeTentacles.Remove(tentacle);
        _usedPositions.Remove(tentacle.transform.position);
    }

    private IEnumerator TentacleSpawner()
    {
        while (true)
        {
            if(_activeTentacles.Count < _maxTentacles)
            {
                SpawnTentacle();
                print("Could spawn a tentacle, yipsers");
            }

            yield return new WaitForSeconds(_spawnTentacleAttempt);
        }
    }

    //Returns a rrandom position that isnt occupated yet
    private Transform RandomTentacleSpawnPos
    {
        get
        {
            var availablePositions = tentacleSpawnPositions
                .Where(pos => !_usedPositions.Contains(pos.position))
                .ToList();

            if (availablePositions.Count == 0)
                return null;

            var randomPos = availablePositions[Random.Range(0, availablePositions.Count)];
            _usedPositions.Add(randomPos.position);

            if(randomPos == null)
            {
                Debug.LogWarning("RandomTentacleSpawnPos returned null, no available positions left.");
                return null;
            }

            return randomPos;
        }
    }

    private void SpawnTentacle()
    {
        var tentacleSpawnPos = RandomTentacleSpawnPos;
        if(tentacleSpawnPos == null)
        {
            Debug.LogWarning("No available tentacle spawn position found.");
            return;
        }
        var tentacleGO = InstantiateChild(_tentacleGO, tentacleSpawnPos);
        tentacleGO.transform.Rotate(gameObject.name == "Left" ? new Vector3(0, -90, 0) : new Vector3(0, 90, 0));

        tentacleGO.transform.parent = tentacleSpawnPos;
        _activeTentacles.Add(tentacleGO);
    }

    protected override void OnActorDeath()
    {
        base.OnActorDeath();
    }
}
