using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public struct BossTimeSpawn
{
    public GameObject bossPrefab;
    public int day;
    public Transform customTransform;
    public Vector3 additiveRotation;
}
public class BossManager : MonoBehaviour
{
    public List<BossTimeSpawn> bossTimeSpawns = new List<BossTimeSpawn>();

    public List<Transform> tentacleSpawnPositions = new List<Transform>();

    [SerializeField] private int endDay;


    private void Start()
    {
        TimeEventManager.Instance.OnDayEnd.AddListener(OnDayStart);
    }

    public void OnDayStart(int day)
    {
        foreach (BossTimeSpawn bossSpawn in bossTimeSpawns)
        {
            if (day  == bossSpawn.day)
            {
                SpawnBoss(bossSpawn);
            }
        }

        if (day == endDay)
        {
            WinManager.Instance.HandleWinning();
        }
    }

    private void SpawnBoss(BossTimeSpawn bossSpawn)
    {
       var boss =  Instantiate(bossSpawn.bossPrefab, bossSpawn.customTransform.position, bossSpawn.customTransform.rotation, bossSpawn.customTransform);
        if(bossSpawn.additiveRotation != Vector3.zero)
        {
            bossSpawn.customTransform.Rotate(bossSpawn.additiveRotation);
        }

        if(boss.TryGetComponent(out Kraken kraken))
        {
            kraken.tentacleSpawnPositions = tentacleSpawnPositions;
        }
    }

}
