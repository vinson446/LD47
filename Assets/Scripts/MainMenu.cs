using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [Header("Visual Settings")]
    public float fadeInTime;

    [Header("Text References")]
    public TextMeshProUGUI gameNameText;
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

        StartCoroutine(Fade(1));
    }

    IEnumerator Fade(float alpha)
    {
        gameNameText.DOFade(1, fadeInTime);

        yield return new WaitForSeconds(fadeInTime);

        playText.DOFade(1, fadeInTime);
        quitText.DOFade(1, fadeInTime);
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

        gameNameText.DOFade(0, fadeInTime);
        playText.DOFade(0, fadeInTime);
        quitText.DOFade(0, fadeInTime);

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
