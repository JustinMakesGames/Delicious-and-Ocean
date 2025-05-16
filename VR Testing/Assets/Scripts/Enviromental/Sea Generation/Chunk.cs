using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public unsafe class Chunk : MonoBehaviour
{
    [SerializeField] private Transform _waterPos;

    [SerializeField] private List<GameObject> _generatedObjects = new List<GameObject>();

    public void Init(GameObject waterGO, int chunkSize, List<GenerationObject> generationObjects, int generationObjectCount)
    {
        var water = Instantiate(waterGO, _waterPos.position, Quaternion.identity, transform);

        //Temporary division by 1000, the water import is fucked
        water.transform.localScale = new Vector3(chunkSize / 100_0, 1, chunkSize / 100_0);

        SeedChunk(chunkSize, generationObjects, generationObjectCount);
    }

    //Seeds the chunk with objects
    private unsafe void SeedChunk(int chunkSize, List<GenerationObject> generationObjects, int generationObjectCount)
    {
        //Init the list of generation objects
        List<GenerationObject> _generationObjects = new List<GenerationObject>();
        for (int i = 0; i < generationObjects.Count; i++)
        {
            _generationObjects.Add(generationObjects[i]);
        }

        //Generate the objects
        for (int i = 0; i < generationObjectCount; i++)
        {
            int randomIndex = Random.Range(0, _generationObjects.Count);
            GenerationObject randomGenerationObject = _generationObjects[randomIndex];

            Vector3 GetRandomPos()
            {
                Vector3 randomPosition = randomGenerationObject.setRelativeObjectPos != Vector3.zero
    ? randomGenerationObject.setRelativeObjectPos + transform.position
    : new Vector3(Random.Range(-chunkSize / 2, chunkSize / 2), 0, Random.Range(-chunkSize / 2, chunkSize / 2))
    + transform.position
    + randomGenerationObject.relativeObjectAdditivePos;
                return randomPosition;
            }

            GameObject generatedObject = Instantiate(randomGenerationObject.objectGO, GetRandomPos(), Quaternion.identity, transform);

            if (randomGenerationObject.multipleInstancesAllowed == false)
            {
                _generationObjects.RemoveAt(randomIndex);
            }
            _generatedObjects.Add(generatedObject);
        }
    }
}
