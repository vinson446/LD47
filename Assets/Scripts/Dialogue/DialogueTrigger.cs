using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Settings")]
    public Dialogue[] dialogue;
    DialogueManager dialogueManager;

    GameManager gameManager;

    private void Awake()
    {
        dialogueManager = GetComponent<DialogueManager>();    
    }

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        TriggerGameStartDialogue();
    }

    public void TriggerGameStartDialogue()
    {
        if (!gameManager.hasFailed)
            dialogueManager.StartDialogue(dialogue[0]);
        else
            dialogueManager.StartDialogue(dialogue[1]);
    }
}
