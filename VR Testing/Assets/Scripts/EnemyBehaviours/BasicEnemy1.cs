using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JumpArcScript : BaseEnemy
{
    [SerializeField] private Transform fishModel;
    private Vector3 _originalPosition;


    public enum EnemyState
    {
        IdleState,
        PlayerSpottedState,
        EnemyAttacks
    }

    public EnemyState enemyState;
    [Header("NormalFishStats")]
    [SerializeField] private float enemyRange;
    [SerializeField] private float fishSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 _endPosition;

    [Header("PlayerSpotted Variables")]
    [SerializeField] private Transform boat;

    [Header("JumpArc Variables")]
    [SerializeField] private float jumpDistance;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpDuration;
    private bool _hasSpottedShip;
    protected override void Awake()
    {
        base.Awake();
        fishModel = transform.GetChild(0);
    }

    private void Start()
    {
        //_originalPosition = transform.position;
        //StateChange(enemyState);

        Arc();
    }

    private void Arc()
    {
        StartCoroutine(Arcing());
    }

    private IEnumerator Arcing()
    {
        while (true)
        {
            yield return StartCoroutine(JumpArc(transform.position, transform.position + transform.forward * jumpDistance, jumpHeight, jumpDuration));
        }
    }
    /*private void Update()
    {
        if (!_hasSpottedShip)
        {
            HandleIdleState();
        }
    }*/
    private void StateChange(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.IdleState:
                IdleStart();
                break;
            case EnemyState.PlayerSpottedState:
                PlayerSpottedStart();
                break;
            
                
        }
    }

    private void HandleIdleState()
    {
        Vector3 direction = (_endPosition - transform.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction);

        fishModel.rotation = Quaternion.Lerp(fishModel.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.Translate(direction * fishSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _endPosition) < 0.1f) 
        {
            _endPosition = SearchPosition();
        }
    }
    private void IdleStart()
    {
        _endPosition = SearchPosition();
    }

    private Vector3 SearchPosition()
    {
        Vector3 randomPosition = new Vector3(Random.Range(_originalPosition.x - enemyRange, _originalPosition.x + enemyRange), transform.position.y,
            Random.Range(_originalPosition.z - enemyRange, _originalPosition.z + enemyRange));

        return randomPosition;
    }

    private void PlayerSpottedStart()
    {
        
    }

    private void HandlePlayerSpotted()
    {
        
    }

    private IEnumerator JumpArc(Vector3 startPos, Vector3 endPos, float height, float duration)
    {
        float time = 0;

        while (time < duration)
        {
            float t = time / duration;

            Vector3 midPoint = Vector3.Lerp(startPos, endPos, t);
            midPoint.y += height * 4 * (t - t * t);

            transform.position = midPoint;

            time += Time.deltaTime;
            yield return null;
        }


    }

  
}
