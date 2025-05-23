using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class FishEnemy : BaseEnemy
{
    [SerializeField] private float rotateAroundSpeed;
    [SerializeField] private float fishSpeed;
    [SerializeField] private GameObject food;
    [SerializeField] private float intervalTime;
    [SerializeField] private float rotationSpeed;

    private Vector3 _destination;
    private Quaternion _rotationToLookAt;
    private Transform _worldPool;
    private bool _hasPlayerPassed;
    private bool _isMovingDifferently;

    protected override void Awake()
    {
        base.Awake();
        _worldPool = transform.parent;
    }

    private void Start()
    {
        StartCoroutine(ChooseMovement());
    }

    private IEnumerator ChooseMovement()
    {
        while (true)
        {
            int randomOption = Random.Range(0, 3);

            if (randomOption == 0)
            {
                _isMovingDifferently = true;
                ChooseRandomPosition();
            }

            else
            {
                _isMovingDifferently = false;
                transform.LookAt(_worldPool.transform.position);
            }

            yield return new WaitForSeconds(intervalTime);
        }
        
    }


    private void Update()
    {
        if (!_hasPlayerPassed)
        {
            Movement();
        }
    }

    private void Movement()
    {
        if (!_isMovingDifferently)
        {
            transform.RotateAround(_worldPool.position, Vector3.up, rotateAroundSpeed * Time.deltaTime);
        }

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _destination, fishSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Lerp(transform.rotation, _rotationToLookAt, rotationSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _destination) < 0.1f)
            {
                ChooseRandomPosition();
            }
        }
    }

    private void ChooseRandomPosition()
    {
        Bounds bounds = _worldPool.GetComponent<Collider>().bounds;
        _destination = new Vector3(Random.Range(bounds.min.x, bounds.max.x), _worldPool.position.y, Random.Range(bounds.min.z, bounds.max.z));

        Vector3 direction = _destination - transform.position;
        direction.Normalize();
        _rotationToLookAt = Quaternion.LookRotation(direction);


    }
}
