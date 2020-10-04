using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMGame : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(FadeInBGM());
    }

    IEnumerator FadeInBGM()
    {
        float currentTime = 0;
        while (currentTime < 2)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0, 1, currentTime / 2);

            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
