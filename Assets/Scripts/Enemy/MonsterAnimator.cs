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

    [Header("Sound Settings")]
    public float moveVolume;
    public float movePitch;
    public float moveInterval;
    public float sprintingVolume;
    public float sprintingPitch;
    public float sprintingInterval;

    public float roarCooldown;
    public bool firstTimeRoaring = false;
    public bool canRoarAgain = true;

    public AudioClip[] audioClips;
    public AudioSource roarSource;
    AudioSource audioSource;

    private void Awake()
    {
        monster = GetComponent<Monster>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        player = FindObjectOfType<Player>();
    }

    void OnIdle()
    {
        animator.CrossFadeInFixedTime(idleState, 0.2f);
    }

    void OnMove()
    {
        animator.CrossFadeInFixedTime(moveState, 0.2f);

        PlayMovingSound(0, moveVolume, movePitch);
    }

    void OnSprinting()
    {
        animator.CrossFadeInFixedTime(sprintState, 0.2f);
        player.timerForAggro = Random.Range(0f, 3f);

        PlayMovingSound(0, sprintingVolume, sprintingPitch);
    }

    void OnRoaring()
    {
        animator.CrossFadeInFixedTime(roarState, 0.2f);
    }

    void PlayMovingSound(int index, float volume, float pitch)
    {
        StopAllCoroutines();
        StartCoroutine(PlaySoundCoroutine(index, volume, pitch));
    }

    public void PlayRoarSound()
    {
        if (!firstTimeRoaring)
        {
            roarSource.PlayOneShot(audioClips[1]);

            firstTimeRoaring = true;
            canRoarAgain = false;
            Invoke("RoarCD", roarCooldown);
        }
        else
        {
            float tmp = Random.Range(0, 4);
            if (tmp == 0 && canRoarAgain)
            {
                roarSource.PlayOneShot(audioClips[1]);

                canRoarAgain = false;
                Invoke("RoarCD", roarCooldown);
            }
        }
    }

    public void RoarCD()
    {
        canRoarAgain = true;
    }

    IEnumerator PlaySoundCoroutine(int index, float volume, float pitch)
    {
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.clip = audioClips[index];

        while (true)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.Play();

            if (!monster.isAggro)
            {
                yield return new WaitForSeconds(moveInterval);
            }
            else
            {
                yield return new WaitForSeconds(sprintingInterval);
            }
        }
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
