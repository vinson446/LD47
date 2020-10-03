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

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

            if (Input.GetKey(KeyCode.Space))
            {
                moveDirection.y = jumpSpeed;
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
}
