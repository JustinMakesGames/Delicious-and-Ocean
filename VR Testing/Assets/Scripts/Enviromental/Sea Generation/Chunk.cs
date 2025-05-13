using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private Transform _waterPos;

    public void Init(GameObject waterGO, int chunkSize)
    {
        var water = Instantiate(waterGO, _waterPos.position, Quaternion.identity, transform);

        //Temporary division by 1000, the water import is fucked
        water.transform.localScale = new Vector3(chunkSize / 100_0, 1, chunkSize / 100_0);
    }

}
