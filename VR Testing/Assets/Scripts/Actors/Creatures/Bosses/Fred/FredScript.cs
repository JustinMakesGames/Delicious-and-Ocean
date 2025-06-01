using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
public enum EnemyState
{
    IdleState,
    PlayerSpottedState,
    EnemyAttacks
}

public class Fred : BaseEnemy
{
    [Header("Basic Enemy Variables")]
    [Tooltip("The transform of the fish, used for rotation and position changes")]
    [SerializeField] private Transform _fishTransform;
    [SerializeField] private Animator animator;
    private Vector3 _originalPosition;

    [Header("MovementVars")]
    [SerializeField] private float enemyRange;
    [SerializeField] private float fishSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 _endPosition;

    [Header("PlayerSpotted Variables")]
    [SerializeField] private Transform boat;
    [Tooltip("The range at which the fish will spot the boat and start moving towards it")]
    [SerializeField] private float boatRange;

    [Tooltip("The transform of the fish's mesh")]
    private Transform _fishPosition;
    private bool _hasSpottedShip;
    public EnemyState enemyState;

    [Header("JumpArc Variables")]
    [SerializeField] private float jumpDistance;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpDuration;
    [SerializeField] private float downSpeed;
    [SerializeField] private float upSpeed;
    [Tooltip("The positions where the fish can jump over the boat")]
    [SerializeField] private List<Transform> fishLeftPositions;
    [SerializeField] private List<Transform> fishRightPositions;

    [Header("Ball Variables")]
    [SerializeField] private GameObject ball;
    [SerializeField] private Transform ballSpawnPosition;
    [SerializeField] private Transform player;
    private bool _isArcing;
    private bool _isOnOtherSide;

    protected override void Awake()
    {
        base.Awake();
        _fishTransform = transform.GetChild(0);
    }

    protected void Start()
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

        _fishTransform.rotation = Quaternion.Lerp(_fishTransform.rotation, rotation, rotationSpeed * Time.deltaTime);
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
        _fishPosition = FindRandomBoatPosition();
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

        _fishTransform.rotation = Quaternion.Lerp(_fishTransform.rotation, rotation, rotationSpeed * Time.deltaTime);
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

        float spawnTimer = 0;
        float endSpawnTimer = 1;
        while (time < duration)
        {
            float t = time / duration;

            Vector3 midPoint = Vector3.Lerp(_fishPosition.position, _fishPosition.GetChild(0).position, t);
            midPoint.y += height * 4 * (t - t * t);

            transform.position = midPoint;

            spawnTimer += Time.deltaTime;

            if (spawnTimer > endSpawnTimer)
            {
                spawnTimer = 0;
                ShootBall();
            }
            time += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(1);

        transform.parent = null;

        _isOnOtherSide = !_isOnOtherSide;
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
            case 1:
                StartBreakingBoat();
                break;

        }
    }
    private void StartArcAttack()
    {
        _fishPosition = FindRandomBoatPosition();
        _isArcing = false;
    }

    private Transform FindRandomBoatPosition()
    {
        if (_isOnOtherSide)
        {
            return fishLeftPositions[Random.Range(0, fishLeftPositions.Count)];
        }

        else
        {
            return fishRightPositions[Random.Range(0, fishRightPositions.Count)];
        }
    }

    private void ShootBall()
    {
       GameObject ballClone =  Instantiate(ball, ballSpawnPosition.position, Quaternion.identity);
       
       Vector3 direction = (player.position - ballClone.transform.position).normalized;

        Quaternion rotation = Quaternion.LookRotation(direction);

        ballClone.transform.rotation = rotation;

        ballClone.transform.GetComponent<BallBehaviour>().MakeAttackStats(attackPower);

    }

    private void StartBreakingBoat()
    {
        
    }

}
