using System.Collections;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class playaudio : MonoBehaviour
{
    public float fadeDuration ;
    private AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource=transform.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
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
            StartCoroutine(FadeoutAudio(true));


        }
    }

    private IEnumerator FadeAudio(bool fadeIn)
    {
        float timer = 0;
        audioSource.Play();
        while (timer< fadeDuration) {
            audioSource.volume = Mathf.Lerp(0,1,timer/fadeDuration);
            timer+= Time.deltaTime;
            yield return null;
        }
        audioSource.volume=1;
    }
    private IEnumerator FadeoutAudio(bool fadeOut)
    {
        float timer = 0;
        audioSource.Play();
        while (timer < fadeDuration)
        {
            audioSource.volume = Mathf.Lerp(1, 0, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Pause();
    }
    // Update is called once per frame
   
}
