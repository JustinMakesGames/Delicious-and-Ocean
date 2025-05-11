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

    [Tooltip("Holds the chunks that are generated")]
    [SerializeField] private List<GameObject> _chunks = new List<GameObject>();

    [Tooltip("The original position of the sea generator")]
    private Vector3 _startPos;

    private void Awake()
    {
        _startPos = transform.position;
    }

    private void Start()
    {
        GenerateChunks(_startPos);
    }


    private void Update()
    {
        var yes = (transform.position.x % .5f) >= 0 ? true : false;
        print(yes);
        print(transform.position.x % .5f);
        return;

        if (transform.position.x >= _chunkSize * .5f || transform.position.z >= _chunkSize * .5f)
        {
            GenerateChunks(transform.position);
        }
    }

    [ContextMenu("Test generate chunks")]
    private void GenerateChunks(Vector3 originPoint)
    {
        for (int i = 0; i < _offsets.Length; i++)
        {
            Vector3 offset = _offsets[i];

            // Check if the chunk already exists
            for (int j = 0; j < _chunks.Count; j++)
            {
                if (_offsets[i] * _chunkSize == _chunks[j].transform.position - _startPos)
                {
                    return;
                }
            }

            GameObject chunk = new GameObject();
            chunk.transform.position = _startPos + offset * _chunkSize;
            _chunks.Add(chunk);
        }
    }

   
    private void OnDrawGizmos()
    {
        if(_chunks.Count == 0) { return; }

        Gizmos.color = Color.blue;
        for (int i = 0; i < _offsets.Length; i++)
        {
            for(int j = 0; j < _chunks.Count; j++)
            {
                if (_offsets[i] * _chunkSize == _chunks[j].transform.position - _startPos)
                {
                    Gizmos.DrawWireCube(_chunks[i].transform.position, new Vector3(_chunkSize, 1, _chunkSize));
                }
            }
        }
    }
}
