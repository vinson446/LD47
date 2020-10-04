using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Text References")]
    public TextMeshProUGUI playText;
    public TextMeshProUGUI quitText;

    [Header("Color Settings")]
    public Color32 startColor;
    public Color32 hoverColor;
    public Color32 clickColor;
    public float colorTransitionTime;

    bool startGame = false;

    [Header("Audio Settings")]
    public AudioClip[] clips;
    public AudioSource bgm;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HoverStartGame()
    {
        playText.DOColor(hoverColor, colorTransitionTime);
        quitText.DOColor(startColor, colorTransitionTime);
    }

    public void PlayGame()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        startGame = true;
        playText.DOColor(clickColor, colorTransitionTime);

        float currentTime = 0;
        while (currentTime < 2)
        {
            currentTime += Time.deltaTime;
            bgm.volume = Mathf.Lerp(1, 0, currentTime / 2);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Game");
    }

    public void HoverQuitGame()
    {
        quitText.DOColor(hoverColor, colorTransitionTime);
        playText.DOColor(startColor, colorTransitionTime);
    }

    public void QuitGame()
    {
        StartCoroutine(Quit());
    }

    IEnumerator Quit()
    {
        quitText.DOColor(clickColor, colorTransitionTime);

        yield return new WaitForSeconds(1);

        Application.Quit();
    }

    public void ResetColor()
    {
        if (!startGame)
        {
            quitText.DOColor(startColor, colorTransitionTime);
            playText.DOColor(startColor, colorTransitionTime);
        }
    }
}
