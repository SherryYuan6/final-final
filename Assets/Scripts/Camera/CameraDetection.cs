using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class CameraDetection : MonoSingleton<CameraDetection>
{
    [Header("Timing")]
    public float fadeInTime = 0.25f;
    public float fadeOutTime = 0.6f;

    public Color detectedColor = new Color(1f, 0f, 0f, 1f);
    public Color defaultColor = new Color(0f, 0f, 0f, 1f);
    public float flashFrequency = 2f;

    public float flashDepth = 0.5f;
    private static readonly int ID_Intensity = Shader.PropertyToID("_Intensity");
    private static readonly int ID_Tint = Shader.PropertyToID("_Tint");

    private RawImage _image;
    private Material _mat;
    private Coroutine _fadeRoutine;
    private Coroutine _flashRoutine;
    private float _currentIntensity;
    private bool _detected;

    protected override void Awake()
    {
        base.Awake();
        _image = GetComponent<RawImage>();
        _image.texture = Texture2D.whiteTexture;
        _mat = new Material(Shader.Find("Custom/UI/RedFlashOverlay"));
        _image.material = _mat;
        SetIntensity(0f);
        SetTint(defaultColor);
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
        SetTint(detectedColor);
        RestartFade(1f, fadeInTime);
        StartFlashing();
    }

    public void EndDetection()
    {
        if (!_detected) return;
        _detected = false;
        StopFlashing();
        SetTint(defaultColor);
        RestartFade(0f, fadeOutTime);
    }

    public void FlashOnce(float holdSeconds = 0.4f)
    {
        if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
        _fadeRoutine = StartCoroutine(OneShot(holdSeconds));
    }
    private void StartFlashing()
    {
        StopFlashing();
        _flashRoutine = StartCoroutine(FlashLoop());
    }

    private void StopFlashing()
    {
        if (_flashRoutine != null)
        {
            StopCoroutine(_flashRoutine);
            _flashRoutine = null;
        }
    }

    private IEnumerator FlashLoop()
    {
        while (true)
        {
          
            if (_fadeRoutine != null)
            {
                yield return null;
                continue;
            }

            float t = Time.unscaledTime * flashFrequency * Mathf.PI * 2f;
            float wave = (Mathf.Sin(t) * 0.5f + 0.5f);          // 0..1
            float intensity = 1f - flashDepth * (1f - wave);    

            SetIntensity(intensity);
            yield return null;
        }
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

    private void SetTint(Color c)
    {
        _mat.SetColor(ID_Tint, c);
    }
}