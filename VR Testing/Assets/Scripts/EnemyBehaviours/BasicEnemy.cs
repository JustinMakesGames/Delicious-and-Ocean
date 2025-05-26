using System;
using System.Collections;

using UnityEngine;
using Random = UnityEngine.Random;

public class BasicEnemy : BaseEnemy
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform fishModel;
    [SerializeField] private Animator animator;
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
    [SerializeField] private float boatRange;
    [SerializeField] private Transform fishPositions;
    private Transform _fishPosition;

    [Header("JumpArc Variables")]
    
    [SerializeField] private float jumpDistance;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpDuration;
    [SerializeField] private Transform endPositionShip;
    [SerializeField] private float downSpeed;
    [SerializeField] private float upSpeed;

    private bool _isArcing;
    private bool _hasSpottedShip;
    protected override void Awake()
    {
        base.Awake();
        fishModel = transform.GetChild(0);
    }

    private void Start()
    {
        _originalPosition = transform.position;
        StateChange(enemyState);
    }
    private void Update()
    {
        if (!_hasSpottedShip)
        {
            HandleIdleState();
            CheckIfCloseToBoat();
        }

        else
        {
            HandlePlayerSpotted();
        }
    }
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
        _fishPosition = FindFirstBoatPosition();
    }

    private void CheckIfCloseToBoat()
    {
        if (Vector3.Distance(transform.position, boat.position) < boatRange)
        {
            enemyState = EnemyState.PlayerSpottedState;

            StateChange(enemyState);
            _hasSpottedShip = true;
        } 
    }
    private void HandlePlayerSpotted()
    {
        if (_isArcing) return;
        Vector3 direction = (_fishPosition.position - transform.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction);

        fishModel.rotation = Quaternion.Lerp(fishModel.rotation, rotation, rotationSpeed * Time.deltaTime);
        transform.Translate(direction * fishSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _fishPosition.position) < 0.3f)
        {
            Transform endPosition = _fishPosition.GetChild(0);
            Vector3 directionToLook = (endPosition.position - transform.position).normalized;

            Quaternion lookRotation = Quaternion.LookRotation(directionToLook);
            transform.GetChild(0).rotation = lookRotation;
            StartCoroutine(JumpArc(jumpHeight, jumpDuration));
        }
    }

    private IEnumerator JumpArc(float height, float duration)
    {
        _isArcing = true;
        float time = 0;

        transform.parent = boat;

        animator.SetTrigger("StartJump");

        float timeDown = 0;
        while (timeDown < 3)
        {
            transform.Translate(Vector3.down * downSpeed * Time.deltaTime);
            timeDown += Time.deltaTime;

            yield return null;
        }

        float timeUp = 0;

        animator.SetTrigger("Jump");
        while (timeUp < 1f)
        {
            transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
            timeUp += Time.deltaTime;
            yield return null;
        }

        while (time < duration)
        {
            float t = time / duration;

            Vector3 midPoint = Vector3.Lerp(_fishPosition.position, _fishPosition.GetChild(0).position, t);
            midPoint.y += height * 4 * (t - t * t);

            transform.position = midPoint;

            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1);

        transform.parent = null;

        ChooseNextAttack();
    }

    //Choosing what the next attack is, there is only one now so you can make the next one.
    private void ChooseNextAttack()
    {
        int randomAttack = 0;

        switch (randomAttack)
        {
            case 0:
                StartArcAttack();
                break;

        }
    }
    private void StartArcAttack()
    {
        _fishPosition = FindFirstBoatPosition();
        _isArcing = true;
    }

    private Transform FindFirstBoatPosition()
    {
        int randomBoatSide = Random.Range(0, fishPositions.childCount);
        
        Transform returnPosition = fishPositions.GetChild(randomBoatSide);

        return returnPosition;
    }

  
}
