using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public GameObject textGameObject;
    public GameObject objective;
    public GameObject clues;

    public TextMeshProUGUI textBox;
    public Queue<string> sentences;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        sentences = new Queue<string>();    
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && textBox.gameObject.activeInHierarchy)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        textGameObject.SetActive(true);

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        textBox.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textBox.text += letter;
            yield return new WaitForSeconds(0.04f);
        }
    }

    public void EndDialogue()
    {
        textGameObject.SetActive(false);

        if (!gameManager.beatGame)
        {
            objective.SetActive(true);
            clues.SetActive(true);

            Player player = FindObjectOfType<Player>();
            player.enabled = true;

            Monster monster = FindObjectOfType<Monster>();
            monster.enabled = true;
            MonsterAnimator monsterAnimator = monster.GetComponent<MonsterAnimator>();
            monsterAnimator.enabled = true;
        }
    }
}
