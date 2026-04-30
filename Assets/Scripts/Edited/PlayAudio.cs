using System.Collections;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class playAudio : MonoBehaviour
{
    public float fadeDuration = 1f;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(FadeAudio(true));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            StartCoroutine(FadeAudio(false));
        }
    }

    IEnumerator FadeAudio(bool fadeIn)
    {
        float start = audioSource.volume;
        float end;
        if (fadeIn)
        {
            end = 1f;
        }
        else
        {
            end = 0f;
        }
        float timer = 0f;
        if (fadeIn && !audioSource.isPlaying)
            audioSource.Play();
        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(start, end, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = end;
        if (!fadeIn)
            audioSource.Pause();
    }
}