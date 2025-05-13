using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGenerator : MonoBehaviour
{
    [Tooltip("The vector3 array responsible for holding the offsets of the chunks")]
    private Vector3[] _offsets = {
        new Vector3(0, 0, 0),
        new Vector3(0, 0, 1),
        new Vector3(1, 0, 0),
        new Vector3(1, 0, 1),
        new Vector3(0, 0, -1),
        new Vector3(-1, 0, 0),
        new Vector3(-1, 0, -1),
        new Vector3(1, 0, -1),
        new Vector3(-1, 0, 1),
    };

    [Tooltip("The 2-dimensional width and height of the (to be generated) chunks")]
    [SerializeField] private int _chunkSize;

    [SerializeField] private GameObject _chunkPrefab;

    [Tooltip("Holds the chunks that are generated")]
    [SerializeField] private List<GameObject> _chunks = new List<GameObject>();

    private Vector3Int _currentChunkPosition;

    [Tooltip("Holds the positions of chunks that are already generated, for fast lookup")]
    private HashSet<Vector3Int> _existingChunks = new HashSet<Vector3Int>();


    [Tooltip("Chunk init values")]
    [SerializeField] private GameObject _waterPrefab;

    private void Update()
    {
        Vector3Int newChunkPosition = new Vector3Int(Mathf.FloorToInt(transform.position.x / _chunkSize), 0, Mathf.FloorToInt(transform.position.z / _chunkSize));

        if (newChunkPosition != _currentChunkPosition)
        {
            _currentChunkPosition = newChunkPosition;

            GenerateChunks(_currentChunkPosition);

            CleanUpChunks();
        }
    }

    [ContextMenu("Test generate chunks")]
    public void GenerateChunks(Vector3Int originPoint)
    {
        List<Vector3Int> chunkPositions = new List<Vector3Int>();

        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                Vector3Int chunkPosition = new Vector3Int(originPoint.x + x, originPoint.y, originPoint.z + z);

                if (!_existingChunks.Contains(chunkPosition))
                {
                    chunkPositions.Add(chunkPosition);
                }
            }
        }

        foreach (var chunkPosition in chunkPositions)
        {
            // Instantiate the chunk
            GameObject chunk = Instantiate(_chunkPrefab);
            chunk.transform.position = chunkPosition * _chunkSize;

            chunk.GetComponent<Chunk>().Init(_waterPrefab, _chunkSize);

            _chunks.Add(chunk);
            _existingChunks.Add(chunkPosition);
        }
    }

    [Tooltip("Removes chunks that are too far away from the SeaGenerator AKA: Cleaning up")]
    private void CleanUpChunks()
    {
        List<GameObject> chunksToRemove = new List<GameObject>();
        foreach (var chunk in _chunks)
        {
            Vector3 chunkPosition = chunk.transform.position;
            Vector3Int chunkPosInt = new Vector3Int(Mathf.FloorToInt(chunkPosition.x / _chunkSize), 0, Mathf.FloorToInt(chunkPosition.z / _chunkSize));

            // Check if this chunk is too far away from the SeaGenerator AKA more than 2 chunks away
            if (Vector3Int.Distance(chunkPosInt, _currentChunkPosition) >= 2)
            {
                chunksToRemove.Add(chunk);
            }
        }

        foreach (var chunk in chunksToRemove)
        {
            _chunks.Remove(chunk);
            _existingChunks.Remove(new Vector3Int(Mathf.FloorToInt(chunk.transform.position.x / _chunkSize), 0, Mathf.FloorToInt(chunk.transform.position.z / _chunkSize)));
            Destroy(chunk);
        }
    }

    private void OnDrawGizmos()
    {
        if (_chunks.Count == 0) { return; }

        Gizmos.color = Color.blue;
        foreach (var chunk in _chunks)
        {
            Vector3 BCCenter = new Vector3(chunk.transform.position.x, _chunkSize * .5f, chunk.transform.position.z);
            Gizmos.DrawWireCube(BCCenter, new Vector3(_chunkSize, _chunkSize, _chunkSize));
        }
    }
}
