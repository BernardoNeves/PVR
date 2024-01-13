using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour {

    [Header("Animator")]
    public Animator _animator;

    [Header("Prefabs")]
    public GameObject projectilePrefab;
    public GameObject grenadePrefab;
    public GameObject bigProjectilePrefab;
    public GameObject explosionPrefab;

    [Header("Enemy Chance Spawn")]
    public int enemyChanceSpawn;

    [Header("Enemy Minions Boss")]
    public GameObject minionPrefab;
    public float timeBetweenSummon = 10f;
    private float timeSinceLastSummon = 0f;
    public int enemySummonQuantity = 5;

    [Header("Enemy Range")]
    public float shootDistance;
    public float throwDistance;
    public float projectileSpeed;

    [Header("Enemy Melee")]
    public float enemyDamage;
    public EnemyHealth enemyHealth;

    [Header("Enemy Type")]
    [Tooltip("1- Ranged\n" +
        "2- Grenade\n" +
        "3- Minion Boss\n" +
        "4- Meele\n" +
        "5- Lifesteal\n" +
        "6- Terrorist\n")]
    public int enemyType = 1;

    [Header("Enemy")]
    private Transform playerTransform;
    public float timeBetweenShot = 1f;
    private float timeSinceLastShot = 0f;

    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 4;
    public float speedRun = 6;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1.0f;
    public int edgeInteractions = 4;

    public Transform[] waypoints;
    private int _currentWaypointIndex;

    Vector3 playerLastPos = Vector3.zero;
    Vector3 _playerpos;

    private float _waitTime;
    private float _timeToRotate;
    private bool _playerInRange;
    private bool _playerNear;
    private bool _isPatrol;
    private bool _caughtPlayer;

    private int xPos;
    private int zPos;

    Vector3 _playerPos;


    void Start () {

        playerTransform = GameManager.instance.Player.transform;

        _playerPos = playerTransform.position;
        _isPatrol = true;
        _caughtPlayer = false;
        _playerInRange = false;
        _playerNear = false;
        _waitTime = startWaitTime;
        _timeToRotate = timeToRotate;

        _currentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(_playerPos);

    }

    void Update() {

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

        EnviromentView();

        if (distanceToPlayer <= shootDistance) {

            Shoot();

            if (enemyType == 3)
            {

                SpawnMinions();

            }

            Stop();

            Vector3 targetpos = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
            transform.LookAt(targetpos);

        } else if (!_isPatrol) {

            ChasePlayer();

            Vector3 targetpos = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z);
            transform.LookAt(targetpos);

        } else  {

            Patroling();

        }

        timeSinceLastShot += Time.deltaTime;
        timeSinceLastSummon += Time.deltaTime;

    }

    void EnviromentView()
    {

        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {

            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle)
            {

                float distToPlayer = Vector3.Distance(transform.position, player.position);

                if (!Physics.Raycast(transform.position, dirToPlayer, distToPlayer, obstacleMask))
                {

                    _playerInRange = true;
                    _isPatrol = false;

                }
                else
                {

                    _playerInRange = false;

                }

            }

            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {

                _playerInRange = false;

            }

            if (_playerInRange)
            {

                _playerPos = playerTransform.position;

            }

        }

    }

    private void Patroling() {

        if (_playerNear) {

            if (_timeToRotate <= 0) {

                Move(speedWalk);
                LookingPlayer(playerLastPos);

            } else {

                Stop();
                _timeToRotate -= Time.deltaTime;

            }

        } else {

            _playerNear = false;
            playerLastPos = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);

            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {

                if (_waitTime <= 0) {

                    NextPoint();
                    Move(speedWalk);
                    _waitTime = startWaitTime;

                } else {

                    Stop();
                    _waitTime -= Time.deltaTime;

                }

            }

        }

    }

    void ChasePlayer() {

        _playerNear = true;
        playerLastPos = Vector3.zero;
        
        if (!_caughtPlayer) {

            Move(speedRun);
            navMeshAgent.SetDestination(_playerPos);

        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {

            if (_waitTime <= 0 && !_caughtPlayer && Vector3.Distance(transform.position, playerTransform.position) >= 6f)
            {

                _isPatrol = true;
                _playerNear = false;
                Move(speedWalk);
                _timeToRotate = timeToRotate;
                _waitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);

            } else {

                if (Vector3.Distance(transform.position, playerTransform.position) >= 2.5f) {

                    Stop();
                    _waitTime -= Time.deltaTime;

                }

            }

        }

    }

    void LookingPlayer(Vector3 player)
    {

        navMeshAgent.SetDestination(player);

        if (Vector3.Distance(transform.position, player) <= 0.3)
        {

            if (_waitTime <= 0)
            {

                _playerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);
                _waitTime = startWaitTime;
                _timeToRotate = timeToRotate;

            }
            else
            {

                Stop();
                _waitTime -= Time.deltaTime;

            }

        }

    }

    void Move(float speed) {
        _animator.SetBool("Walk", true);
        _animator.SetBool("Run", false);

        if (speed == speedRun)
            _animator.SetBool("Run", true);

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;

    }

    void Stop() {
        _animator.SetBool("Walk", false);
        _animator.SetBool("Run", false);

        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;

    }

    public void NextPoint() {

        _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[_currentWaypointIndex].position);

    }

    private void Shoot() {

        if (CanShot()) {

            if (enemyType == 1)
            {
                Vector3 throwOffset = new Vector3(0f, 1.025f, 0.1f);
                Vector3 playerDirection = (playerTransform.position - transform.position).normalized;
                GameObject projectile = Instantiate(projectilePrefab, transform.position + throwOffset, Quaternion.LookRotation(playerDirection));

                playerDirection.x *= projectileSpeed;
                playerDirection.y *= projectileSpeed;
                playerDirection.z *= projectileSpeed;

                projectile.GetComponent<Rigidbody>().velocity = playerDirection;

                timeSinceLastShot = 0;

            }
            else if (enemyType == 2)
            {

                Vector3 throwOffset = new Vector3(0f, 1.2f, 0f);
                Vector3 playerDirection = (playerTransform.position - transform.position).normalized;

                playerDirection.y += 0.5f;

                GameObject grenade = Instantiate(grenadePrefab, transform.position + throwOffset, Quaternion.LookRotation(playerDirection));
                Rigidbody rigidbody = grenade.GetComponent<Rigidbody>();
                rigidbody.AddForce(playerDirection * throwDistance, ForceMode.VelocityChange);

                timeSinceLastShot = 0;

            }
            else if (enemyType == 3)
            {

                Vector3 playerDirection = (playerTransform.position - transform.position).normalized;
                GameObject projectile = Instantiate(bigProjectilePrefab, transform.position, Quaternion.LookRotation(playerDirection));

                playerDirection.x *= 10f;
                playerDirection.y -= 0.1f;
                playerDirection.z *= 10f;

                projectile.GetComponent<Rigidbody>().velocity = playerDirection;

                timeSinceLastShot = 0;

            }
            
        }

    }

    private bool CanShot() {
        _animator.SetTrigger("Attack");

        if (timeSinceLastShot < timeBetweenShot) {

            return false;

        } else {

            return true;

        }

    }

    private void SpawnMinions() {

        if(CanSummon()) {

            for (int i = 0; i < enemySummonQuantity; i++) {

                xPos = Random.Range(-5, 6);
                zPos = Random.Range(-5, 6);

                Vector3 throwOffset = new Vector3(xPos, 0f, zPos);

                Instantiate(minionPrefab, transform.position + throwOffset, Quaternion.identity, transform);

            }

            timeSinceLastSummon = 0;

        }

    }

    private bool CanSummon() {

        if (timeSinceLastSummon < timeBetweenSummon) {

            return false;

        } else {

            return true;

        }

    }

    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.tag == "Player")
        {

            if (enemyType == 4)
            {

                if (CanShot())
                {

                    GameManager.instance.PlayerHealth.Damage(enemyDamage);

                    timeSinceLastShot = 0;
                }

            }
            else if (enemyType == 5)
            {

                if (CanShot())
                {

                    GameManager.instance.PlayerHealth.Damage(enemyDamage);

                    enemyHealth.Heal(enemyDamage);

                    timeSinceLastShot = 0;

                }

            }
            else if (enemyType == 6)
            {

                Explode();

            }

        }

    }

    private void Explode()
    {

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);

    }

}