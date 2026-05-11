using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class CameraDetection : MonoSingleton<CameraDetection>
{
    [Header("Timing")]
    public float fadeInTime = 0.25f;
    public float fadeOutTime = 0.6f;

    private static readonly int ID_Intensity = Shader.PropertyToID("_Intensity");

    private RawImage _image;
    private Material _mat;
    private Coroutine _fadeRoutine;
    private float _currentIntensity;
    private bool _detected;

    protected override void Awake()
    {
        base.Awake();

        _image = GetComponent<RawImage>();
        _mat = Instantiate(_image.material);
        _image.material = _mat;

        SetIntensity(0f);
        _image.raycastTarget = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_mat != null) Destroy(_mat);
    }

    public void StartDetection()
    {
        if (_detected) return;
        _detected = true;
        RestartFade(1f, fadeInTime);
    }

    public void EndDetection()
    {
        if (!_detected) return;
        _detected = false;
        RestartFade(0f, fadeOutTime);
    }

    public void FlashOnce(float holdSeconds = 0.4f)
    {
        if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(OneShot(holdSeconds));
    }

    private void RestartFade(float targetIntensity, float duration)
    {
        if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(Fade(targetIntensity, duration));
    }

    private IEnumerator Fade(float target, float duration)
    {
        float start = _currentIntensity;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            SetIntensity(Mathf.Lerp(start, target, t / duration));
            yield return null;
        }

        SetIntensity(target);
        _fadeRoutine = null;
    }

    private IEnumerator OneShot(float holdSeconds)
    {
        yield return Fade(1f, fadeInTime);
        yield return new WaitForSecondsRealtime(holdSeconds);
        yield return Fade(0f, fadeOutTime);
        _fadeRoutine = null;
    }

    private void SetIntensity(float v)
    {
        _currentIntensity = v;
        _mat.SetFloat(ID_Intensity, v);
        _image.enabled = v > 0.001f;
    }
}