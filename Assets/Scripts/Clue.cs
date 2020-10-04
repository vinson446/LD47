using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    public int index;

    DialogueManager dialogueManager;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUpClue()
    {
        dialogueManager.clues.gameObject.transform.GetChild(index).gameObject.SetActive(true);

        gameManager.numCluesFound += 1;

        gameObject.SetActive(false);
    }
}
