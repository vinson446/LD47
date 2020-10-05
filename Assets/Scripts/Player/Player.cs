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

    public bool seeMonsterFirstTime;
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
    public bool startMoveSound = false;

    public AudioClip[] audioClips;
    AudioSource audioSource;
    public AudioSource aSource;

    [Header("References")]
    public Monster monster;
    public MonsterAnimator monsterAnimator;
    public Collider monsterCollider;
    public Camera cam;

    Plane[] planes;

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
        monsterOnScreen = IsInView(gameObject, monster.gameObject);

        /*
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (GeometryUtility.TestPlanesAABB(planes, monsterCollider.bounds))
        {
            monsterOnScreen = true;
        }
        else
        {
            monsterOnScreen = false;
        }
        */

        if (monsterOnScreen)
        {
            timer += Time.deltaTime;
            if (timer >= timerForAggro)
            {
                timer = 0;
                monster.isAggro = true;
            }

            if (!seeMonsterFirstTime && monsterAnimator.enabled && Vector3.Distance(transform.position, monster.transform.position) < 20)
            {
                monsterAnimator.PlayRoarSound();

                seeMonsterFirstTime = true;
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

                startMoveSound = false;
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

        if (isMoving)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                PlaySound(0, Random.Range(sprintingVolume - 0.03f, sprintingVolume + 0.03f), Random.Range(sprintingPitch - 0.03f, sprintingPitch + 0.03f));
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                PlaySound(0, Random.Range(moveVolume - 0.03f, moveVolume + 0.03f), Random.Range(movePitch - 0.03f, movePitch + 0.03f));
            }
            // start move sound
            else if (!startMoveSound)
            {
                PlaySound(0, Random.Range(moveVolume - 0.03f, moveVolume + 0.03f), Random.Range(movePitch - 0.03f, movePitch + 0.03f));
                startMoveSound = true;
            }
        }

        /*
        // play moving sound while player is moving
        if (charController.isGrounded && isMoving && canPlaySound)
        {
            if (isWalking)
            {
                PlaySound(0, Random.Range(moveVolume - 0.03f, moveVolume + 0.03f), Random.Range(movePitch - 0.03f, movePitch + 0.03f));
            }
            else
            {
                PlaySound(0, Random.Range(sprintingVolume - 0.03f, sprintingVolume + 0.03f), Random.Range(sprintingPitch - 0.03f, sprintingPitch + 0.03f));
            }
        }
        */

        Interact();
    }

    public void Breathe()
    {
        aSource.PlayOneShot(audioClips[1]);
    }

    private bool IsInView(GameObject origin, GameObject toCheck)
    {
        Vector3 pointOnScreen = cam.WorldToScreenPoint(toCheck.GetComponentInChildren<Renderer>().bounds.center);

        //Is in front
        if (pointOnScreen.z < 0)
        {
            // Debug.Log("Behind: " + toCheck.name);
            return false;
        }

        //Is in FOV
        if ((pointOnScreen.x < 0) || (pointOnScreen.x > Screen.width) ||
                (pointOnScreen.y < 0) || (pointOnScreen.y > Screen.height))
        {
            // Debug.Log("OutOfBounds: " + toCheck.name);
            return false;
        }

        RaycastHit hit;
        Vector3 heading = toCheck.transform.position - origin.transform.position;
        Vector3 direction = heading.normalized;// / heading.magnitude;

        if (Physics.Linecast(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, out hit))
        {
            if (hit.transform.name != toCheck.name)
            {
                /* -->
                Debug.DrawLine(cam.transform.position, toCheck.GetComponentInChildren<Renderer>().bounds.center, Color.red);
                Debug.LogError(toCheck.name + " occluded by " + hit.transform.name);
                */
                //Debug.Log(toCheck.name + " occluded by " + hit.transform.name);
                return false;
            }
        }
        return true;
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
        StopAllCoroutines();
        StartCoroutine(PlaySoundCoroutine(index, volume, pitch));
    }

    IEnumerator PlaySoundCoroutine(int index, float volume, float pitch)
    {
        audioSource.clip = audioClips[index];
        audioSource.volume = volume;
        audioSource.pitch = pitch;

        while (isMoving)
        {
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.Play();

            if (isWalking)
                yield return new WaitForSeconds(moveInterval);
            else
                yield return new WaitForSeconds(sprintingInterval);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
