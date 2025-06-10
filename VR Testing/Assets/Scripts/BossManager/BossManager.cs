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
}
public class BossManager : MonoBehaviour
{
    public List<BossTimeSpawn> bossTimeSpawns = new List<BossTimeSpawn>();

    
   
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
    }

    private void SpawnBoss(BossTimeSpawn bossSpawn)
    {
        Instantiate(bossSpawn.bossPrefab);
    }

}
