using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimator : MonoBehaviour
{
    const string idleState = "Idle";
    const string moveState = "Move";
    const string sprintState = "Sprinting";
    const string roarState = "Roaring";

    Monster monster;
    Animator animator;

    Player player;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        animator = GetComponent<Animator>();

        player = FindObjectOfType<Player>();
    }

    void OnIdle()
    {
        animator.CrossFadeInFixedTime(idleState, 0.2f);
    }

    void OnMove()
    {
        animator.CrossFadeInFixedTime(moveState, 0.2f);
    }

    void OnSprinting()
    {
        animator.CrossFadeInFixedTime(sprintState, 0.2f);
        player.timerForAggro = Random.Range(0f, 3f);
    }

    void OnRoaring()
    {
        animator.CrossFadeInFixedTime(roarState, 0.2f);
    }

    private void OnEnable()
    {
        monster.MonsterIdle += OnIdle;
        monster.MonsterMove += OnMove;
        monster.MonsterSprinting += OnSprinting;
        monster.MonsterRoar += OnRoaring;
    }

    private void OnDisable()
    {
        monster.MonsterIdle -= OnIdle;
        monster.MonsterMove -= OnMove;
        monster.MonsterSprinting -= OnSprinting;
        monster.MonsterRoar -= OnRoaring;
    }
}
