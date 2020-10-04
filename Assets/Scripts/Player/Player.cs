using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;
    public float sprintSpeed;
    public bool isMoving;
    public bool isWalking;

    public float jumpSpeed;
    public float gravity;

    Vector3 moveDirection = Vector3.zero;
    CharacterController charController;

    [Header("Game Settings")]
    public float interactRange;

    public bool monsterOnScreen;
    public float timerForAggro;
    public float timer;

    [Header("Sound Settings")]
    public float moveVolume;
    public float movePitch;
    public float moveInterval;
    public float sprintingVolume;
    public float sprintingPitch;
    public float sprintingInterval;
    public bool canPlaySound = true;

    public AudioClip[] audioClips;
    AudioSource audioSource;

    [Header("References")]
    public Monster monster;
    public Camera cam;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        timerForAggro = Random.Range(0f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPoint = cam.WorldToViewportPoint(monster.transform.position);
        monsterOnScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (monsterOnScreen)
        {
            timer += Time.deltaTime;
            if (timer >= timerForAggro)
            {
                timer = 0;
                monster.isAggro = true;
            }
        }
        else if (Vector3.Distance(transform.position, monster.transform.position) < 20)
        {
            monster.isAggro = true;
        }
        else
        {
            monster.isAggro = false;
        }

        if (charController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);

            if (moveDirection.x != 0 || moveDirection.z != 0)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                isWalking = false;
                moveDirection = moveDirection * sprintSpeed;
            }
            else
            {
                isWalking = true;
                moveDirection = moveDirection * moveSpeed;
            }
        }

        // apply gravity
        moveDirection.y = moveDirection.y - (gravity * Time.deltaTime);

        // move controller;
        charController.Move(moveDirection * Time.deltaTime);

        // play moving sound while player is moving
        if (charController.isGrounded && isMoving && canPlaySound)
        {
            if (isWalking)
            {
                PlaySound(0, moveVolume, movePitch);
            }
            else
            {
                PlaySound(0, sprintingVolume, sprintingPitch);
            }
        }

        Interact();
    }

    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Collider[] coll = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider c in coll)
            {
                Clue clue = c.GetComponent<Clue>();
                if (clue != null)
                {
                    clue.PickUpClue();
                }
            }
        }
    }

    void PlaySound(int index, float volume, float pitch)
    {
        audioSource.volume = volume;
        audioSource.pitch = pitch;

        audioSource.PlayOneShot(audioClips[index]);
        canPlaySound = false;

        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    { 
        if (isWalking)
        {
            yield return new WaitForSeconds(moveInterval);
            canPlaySound = true;
        }
        else
        {
            yield return new WaitForSeconds(sprintingInterval);
            canPlaySound = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
