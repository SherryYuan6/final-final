using UnityEngine;

public class AmbientWorldAudio : MonoBehaviour
{
    public AudioSource cleanAmbience;
    public AudioSource realAmbience;
    public float crossfadeSpeed = 0.5f;

    void Start()
    {
        if (cleanAmbience != null)
        {
            cleanAmbience.loop = true;
            cleanAmbience.volume = 1f;
            cleanAmbience.Play();
        }

        if (realAmbience != null)
        {
            realAmbience.loop = true;
            realAmbience.volume = 0f;
            realAmbience.Play();
        }
    }

    void Update()
    {
        if (ChipManager.Instance == null) return;

        float broken = 1f - ChipManager.Instance.GetSanityRatio();
        float targetClean = Mathf.Clamp01(1f - broken);
        float targetReal = Mathf.Clamp01(broken * broken);

        if (cleanAmbience != null)
            cleanAmbience.volume = Mathf.MoveTowards(cleanAmbience.volume, targetClean, crossfadeSpeed * Time.deltaTime);

        if (realAmbience != null)
            realAmbience.volume = Mathf.MoveTowards(realAmbience.volume, targetReal, crossfadeSpeed * Time.deltaTime);
    }
}