using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    public int index;

    DialogueManager dialogueManager;
    GameManager gameManager;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        gameManager = FindObjectOfType<GameManager>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUpClue()
    {
        dialogueManager.clues.gameObject.transform.GetChild(index).gameObject.SetActive(true);

        gameManager.numCluesFound += 1;

        audioSource.Play();

        MeshRenderer rend = GetComponent<MeshRenderer>();
        rend.enabled = false;
        Collider col = GetComponent<Collider>();
        col.enabled = false;
    }
}
