using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private Transform _waterPos;

    [SerializeField] private List<GameObject> _generatedObjects = new List<GameObject>();


    private System.Random _randomNumGen;

    public void Init(GameObject waterGO, int chunkSize, ObjectGenerator OG, int generationObjectCount, int seed)
    {
        _randomNumGen = new System.Random(((int)(transform.position.x * transform.position.z) + seed) / 2);
        name = (((int)(transform.position.x * transform.position.z) + seed) / 2).ToString();
        var water = Instantiate(waterGO, _waterPos.position, Quaternion.identity, transform);

        //Temporary division by 1000, the water import is fucked
        water.transform.localScale = new Vector3(chunkSize / 100_0, 1, chunkSize / 100_0);

        SeedChunk(chunkSize, OG, generationObjectCount);
    }

    //Seeds the chunk with objects
    private void SeedChunk(int chunkSize, ObjectGenerator OG, int generationObjectCount)
    {

        //Generate the objects
        for (int i = 0; i < generationObjectCount; i++)
        {

            GenerationObject randomGenerationObject = OG.GetRandomObject(_randomNumGen);

            //Gets a random position within the bounds of the chunk
            Vector3 GetRandomPos()
            {
                Vector3 randomPosition = randomGenerationObject.setRelativeObjectPos != Vector3.zero
    ? randomGenerationObject.setRelativeObjectPos + transform.position
    : new Vector3(_randomNumGen.Next(-chunkSize / 2, chunkSize / 2), 0, _randomNumGen.Next(-chunkSize / 2, chunkSize / 2))
    + transform.position
    + randomGenerationObject.relativeObjectAdditivePos;
                return randomPosition;
            }

            GameObject generatedObject = Instantiate(randomGenerationObject.objectGO, GetRandomPos(), Quaternion.identity, transform);

            _generatedObjects.Add(generatedObject);
        }
    }
}
