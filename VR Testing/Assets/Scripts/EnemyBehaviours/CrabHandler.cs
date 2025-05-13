using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CrabHandler : BaseEnemy
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask boatMask;

    private Transform _player;

    private NavMeshAgent _agent;

    private RaycastHit hitPoint;
    private bool _hasReachedBoat;

    private bool _movesBackwards;
    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        if (!_hasReachedBoat)
        {
            MoveUpBoat();
        }
        else
        {
            MoveToThePlayer();
        }

    }
    private void MoveUpBoat()
    {
        Vector3 origin = transform.position + transform.forward * -1 + Vector3.up * 0.1f;
        Vector3 closeOrigin = transform.position + transform.forward * -0.5f;
        bool canFindBoat = Physics.Raycast(origin, transform.forward, out hitPoint, Mathf.Infinity, boatMask);
        bool rayCastCloseCheck = Physics.Raycast(closeOrigin, transform.forward, out hitPoint, 1, boatMask);

        transform.Translate(Vector3.up * speed * Time.deltaTime);
        if (!rayCastCloseCheck)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        }

        if (!canFindBoat)
        {
            _hasReachedBoat = true;
            MoveCrabToPosition();
        }
    }

    private void MoveCrabToPosition()
    {
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            _agent.enabled = true;
        }
    }

    private void MoveToThePlayer()
    {
        _agent.SetDestination(_player.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            _movesBackwards = true;   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            _movesBackwards = false;
        }
    }




}
