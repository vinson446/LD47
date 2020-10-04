using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour
{
    public event Action MonsterIdle = delegate { };
    public event Action MonsterMove = delegate { };
    public event Action MonsterSprinting = delegate { };
    public event Action MonsterRoar = delegate { };

    [Header("Movement Settings")]
    public float moveSpeed;
    public float sprintSpeed;
    public float usedSpeed;
    public bool isAggro;
    public float increaseSpeedInterval;

    NavMeshAgent agent;
    Transform target;

    [Header("Animation Checks")]
    // animation checks
    public bool isMoving;
    public bool isSprinting;

    // references
    Player player;
    GameManager gameManager;

    MonsterAnimator monsterAnimator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterAnimator = GetComponent<MonsterAnimator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        usedSpeed = moveSpeed;
        agent.speed = usedSpeed;
        StartCoroutine(IncreaseMoveSpeedOverTime());

        target = GameObject.FindGameObjectWithTag("Player").transform;
        player = target.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAggro)
        {
            MoveState();
        }
        else
        {
            SprintState();
        }

        agent.destination = target.position;
    }

    IEnumerator IncreaseMoveSpeedOverTime()
    {
        while (true)
        {
            moveSpeed += 1;
            sprintSpeed += 1;

            if (!isAggro)
            {
                usedSpeed = moveSpeed;
            }
            else
            {
                usedSpeed = sprintSpeed;
            }

            agent.speed = usedSpeed;

            yield return new WaitForSeconds(increaseSpeedInterval);
        }
    }

    // chase player normally
    void MoveState()
    {
        usedSpeed = moveSpeed;
        agent.speed = usedSpeed;

        CheckMoveStateStart();
        CheckSprintStateEnd();
    }

    void CheckMoveStateStart()
    {
        if (!isMoving)
        {
            MonsterMove.Invoke();
            isMoving = true;
        }
    }

    void CheckMoveStateEnd()
    {
        isMoving = false;
    }

    // if monster is in player's camera view, chase player faster
    void SprintState()
    {
        usedSpeed = sprintSpeed;
        agent.speed = usedSpeed;

        CheckSprintStateStart();
        CheckMoveStateEnd();
    }

    void CheckSprintStateStart()
    {
        if (!isSprinting)
        {
            MonsterSprinting.Invoke();

            if (Vector3.Distance(transform.position, target.transform.position) < 50)
                monsterAnimator.PlayRoarSound();

            isSprinting = true;
        }
    }

    void CheckSprintStateEnd()
    {
        isSprinting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameManager = FindObjectOfType<GameManager>();

            if (!gameManager.hasFailed)
            {
                gameManager.hasFailed = true;
            }

            if (gameManager.numCluesFound == 4)
            {
                SceneManager.LoadScene("Main Menu");
            }
            else
            {
                SceneManager.LoadScene("Game");
            }
        }
    }
}
