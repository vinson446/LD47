using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    [Header("Visual Settings")]

    [Header("Game Settings")]
    public int numCluesFound = 0;
    public bool hasFailed = false;

    [Header("Sound Settings")]
    public AudioClip[] audioClips;
    AudioSource audioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        audioSource = GetComponent<AudioSource>();
    }
}
