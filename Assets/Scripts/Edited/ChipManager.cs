using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChipManager : MonoSingleton<ChipManager>
{
    public float maxIntegrity = 100f;
    public float currentIntegrity = 80f;
    public float passiveDecayRate = 1.695f;
    public float revealThreshold = 50f;

    public Slider integritySlider;
    public Image sliderFill;
    public Color healthyColor = Color.green;
    public Color criticalColor = Color.red;
    public Image fadeImage;

    public Material glitchMaterial;

    static readonly int ID_Intensity = Shader.PropertyToID("_Intensity");
    static readonly int ID_BlockSize = Shader.PropertyToID("_BlockSize");
    static readonly int ID_Speed = Shader.PropertyToID("_Speed");
    static readonly int ID_ScanlineStrength = Shader.PropertyToID("_ScanlineStrength");
    static readonly int ID_Flicker = Shader.PropertyToID("_Flicker");

    [Header("Lighting")]
    public Color healthyLightColor = new Color(1f, 0.95f, 0.88f);
    public Color brokenLightColor = new Color(0.3f, 0.9f, 1f);
    public Color healthyAmbient = new Color(0.4f, 0.38f, 0.35f);
    public Color brokenAmbient = new Color(0.05f, 0.12f, 0.2f);

    [Header("Audio")]
    public AudioSource glitchAmbientLoop;
    public AudioSource glitchSpikeSound;

    [Header("Spike Glitch")]
    public float spikeInterval = 8f;
    [Range(0f, 1f)]
    public float spikeProbability = 0.35f;
    public float spikeDuration = 0.4f;

    [Header("Ending")]
    public string endSceneName = "End Screen";

    private Light[] sceneLights;
    private Renderer[] worldRenderers;
    private Material[] originalMaterials;
    private Color[] originalColors;
    private bool[] hasColorProp;

    private GameObject[] fakeClues;
    private GameObject[] trueClues;
    private bool cluesRevealed = false;

    private bool isEnding = false;
    private float spikeTimer = 0f;
    private float spikeTimeLeft = 0f;
    private float baseIntensity = 0f;
    private float spikeIntensity = 0f;

    private Coroutine flickerCoroutine;
    private Coroutine jitterCoroutine;

    void Start()
    {
        CollectSceneObjects();
        InitClues();

        if (fadeImage != null) fadeImage.color = new Color(0, 0, 0, 0);

        if (glitchAmbientLoop != null)
        {
            glitchAmbientLoop.loop = true;
            glitchAmbientLoop.volume = 0f;
            glitchAmbientLoop.Play();
        }

        UpdateHUD();
        PushGlitchToShader(0f, 0f);
        UpdateLighting(0f);
    }

    void Update()
    {
        if (isEnding) return;

        LoseIntegrity(Time.unscaledDeltaTime * passiveDecayRate);

        if (Input.GetKeyDown(KeyCode.K)) LoseIntegrity(10f);

        TickSpike();
    }

    void CollectSceneObjects()
    {

        sceneLights = FindObjectsByType<Light>(FindObjectsSortMode.None);

        var allRenderers = FindObjectsByType<Renderer>(FindObjectsSortMode.None);
        var kept = new List<Renderer>();

        foreach (var r in allRenderers)
        {
            if (r == null) continue;

            if (r.GetComponentInParent<Canvas>() != null) continue;

            if (r is ParticleSystemRenderer) continue;

            if (r.transform.IsChildOf(transform)) continue;
            kept.Add(r);
        }

        worldRenderers = kept.ToArray();
        originalMaterials = new Material[worldRenderers.Length];
        originalColors = new Color[worldRenderers.Length];
        hasColorProp = new bool[worldRenderers.Length];

        for (int i = 0; i < worldRenderers.Length; i++)
        {

            var mat = worldRenderers[i].sharedMaterial;
            originalMaterials[i] = mat;

            if (mat != null && mat.HasProperty("_Color"))
            {
                originalColors[i] = mat.color;
                hasColorProp[i] = true;
            }
        }
    }

    void InitClues()
    {

        fakeClues = FindGameObjectsWithTagSafe("FakeClue");
        trueClues = FindGameObjectsWithTagSafe("TrueClue");

        foreach (var obj in trueClues)
            if (obj != null) obj.SetActive(false);

        foreach (var obj in fakeClues)
            if (obj != null) obj.SetActive(true);
    }

    static GameObject[] FindGameObjectsWithTagSafe(string tag)
    {
        try { return GameObject.FindGameObjectsWithTag(tag); }
        catch { return new GameObject[0]; }
    }

    void CheckClueReveal()
    {
        if (cluesRevealed || !CanRevealTrueClue()) return;

        cluesRevealed = true;

        foreach (var obj in fakeClues)
            if (obj != null) obj.SetActive(false);

        foreach (var obj in trueClues)
            if (obj != null) obj.SetActive(true);

        StartCoroutine(RevealGlitchBurst());
    }

    IEnumerator RevealGlitchBurst()
    {

        for (int i = 0; i < 3; i++)
        {
            if (glitchSpikeSound != null) glitchSpikeSound.Play();
            PushGlitchToShader(IntegrityRatio(), 1f);
            FlickerLightsImmediate();
            yield return new WaitForSecondsRealtime(0.08f);

            PushGlitchToShader(IntegrityRatio(), 0f);
            yield return new WaitForSecondsRealtime(0.06f);
        }
    }

    public void LoseCognitive(float amount) => LoseIntegrity(amount);
    public void AddCognitive(float amount) => AddIntegrity(amount);
    public void RestoreByPercent(float pct) => AddIntegrity(maxIntegrity * pct);
    public float GetSanityRatio() => IntegrityRatio();
    public bool CanRevealTrueClue() => currentIntegrity <= revealThreshold;

    public void LoseIntegrity(float amount)
    {
        currentIntegrity = Mathf.Clamp(currentIntegrity - amount, 0f, maxIntegrity);
        OnChanged();
        if (currentIntegrity <= 0f) TriggerEnding();
    }

    public void AddIntegrity(float amount)
    {
        currentIntegrity = Mathf.Clamp(currentIntegrity + amount, 0f, maxIntegrity);
        OnChanged();
    }

    void OnChanged()
    {
        float ratio = IntegrityRatio();
        float broken = 1f - ratio;

        UpdateHUD();
        baseIntensity = GlitchCurve(broken);
        PushGlitchToShader(ratio, spikeIntensity);
        UpdateLighting(broken);
        UpdateRendererCorruption(broken);
        UpdateAmbientAudio(broken);
        UpdateWorldFlicker(broken);
        CheckClueReveal();
    }

    float IntegrityRatio() => currentIntegrity / maxIntegrity;

    static float GlitchCurve(float broken)
        => Mathf.Clamp01(broken * broken * broken * 1.5f + broken * 0.1f);

    void PushGlitchToShader(float ratio, float extraSpike)
    {
        if (glitchMaterial == null) return;

        float intensity = Mathf.Clamp01(baseIntensity + extraSpike);

        glitchMaterial.SetFloat(ID_Intensity, intensity);
        glitchMaterial.SetFloat(ID_BlockSize, Mathf.Lerp(80f, 8f, intensity));
        glitchMaterial.SetFloat(ID_Speed, Mathf.Lerp(2f, 20f, intensity));
        glitchMaterial.SetFloat(ID_ScanlineStrength, Mathf.Lerp(0f, 1.8f, intensity));

        glitchMaterial.SetFloat(ID_Flicker, Mathf.Sin(intensity * Mathf.PI) * 0.5f);
    }

    void UpdateLighting(float broken)
    {
        Color targetColor = Color.Lerp(healthyLightColor, brokenLightColor, broken);
        float targetIntensity = Mathf.Lerp(1f, 0.55f, broken);

        foreach (var l in sceneLights)
        {
            if (l == null) continue;
            l.color = targetColor;
            l.intensity = targetIntensity;
        }

        RenderSettings.ambientLight = Color.Lerp(healthyAmbient, brokenAmbient, broken);
    }

    void FlickerLightsImmediate()
    {
        float v = Random.value < 0.5f ? 0.1f : 1f;
        foreach (var l in sceneLights)
            if (l != null) l.intensity = v;
    }

    static readonly Color GlitchTint = new Color(0.25f, 1f, 0.85f);

    void UpdateRendererCorruption(float broken)
    {

        float t = Mathf.InverseLerp(0.3f, 1f, broken);
        if (t <= 0f) return;

        for (int i = 0; i < worldRenderers.Length; i++)
        {
            if (!hasColorProp[i] || worldRenderers[i] == null) continue;

            Color target = Color.Lerp(originalColors[i], GlitchTint, t * 0.45f);
            worldRenderers[i].material.color = target;
        }
    }

    void UpdateAmbientAudio(float broken)
    {
        if (glitchAmbientLoop == null) return;
        glitchAmbientLoop.volume = broken * broken;
        glitchAmbientLoop.pitch = Mathf.Lerp(1f, 0.82f, broken);
    }

    void UpdateWorldFlicker(float broken)
    {
        if (broken > 0.25f && flickerCoroutine == null)
            flickerCoroutine = StartCoroutine(WorldFlickerLoop());
        else if (broken <= 0.25f && flickerCoroutine != null)
        {
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;

            foreach (var r in worldRenderers)
                if (r != null) r.enabled = true;
        }

        if (broken > 0.5f && jitterCoroutine == null)
            jitterCoroutine = StartCoroutine(WorldJitterLoop());
        else if (broken <= 0.5f && jitterCoroutine != null)
        {
            StopCoroutine(jitterCoroutine);
            jitterCoroutine = null;
        }
    }

    IEnumerator WorldFlickerLoop()
    {

        var flickeredOff = new List<Renderer>();

        while (true)
        {
            float broken = 1f - IntegrityRatio();
            float interval = Mathf.Lerp(0.5f, 0.06f, broken);
            yield return new WaitForSeconds(interval);

            foreach (var r in flickeredOff)
                if (r != null) r.enabled = true;
            flickeredOff.Clear();

            int count = Mathf.RoundToInt(worldRenderers.Length * broken * 0.15f);
            count = Mathf.Clamp(count, 0, worldRenderers.Length);

            for (int i = 0; i < count; i++)
            {
                int idx = Random.Range(0, worldRenderers.Length);
                var r = worldRenderers[idx];
                if (r == null || !r.enabled) continue;

                r.enabled = false;
                flickeredOff.Add(r);
            }

            yield return new WaitForSeconds(Random.Range(0.03f, 0.12f));

            foreach (var r in flickeredOff)
                if (r != null) r.enabled = true;
            flickeredOff.Clear();
        }
    }

    IEnumerator WorldJitterLoop()
    {

        var origPositions = new Dictionary<Transform, Vector3>();
        foreach (var r in worldRenderers)
        {
            if (r == null) continue;
            var t = r.transform;
            if (!origPositions.ContainsKey(t))
                origPositions[t] = t.position;
        }

        while (true)
        {
            float broken = 1f - IntegrityRatio();
            float interval = Mathf.Lerp(0.3f, 0.05f, broken);
            yield return new WaitForSeconds(interval);

            if (Random.value > broken * 0.4f) continue;

            float mag = Mathf.Lerp(0f, 0.06f, broken);
            int count = Mathf.RoundToInt(origPositions.Count * broken * 0.08f);

            var keys = new List<Transform>(origPositions.Keys);

            for (int i = 0; i < count && i < keys.Count; i++)
            {
                var t = keys[Random.Range(0, keys.Count)];
                if (t == null) continue;
                t.position = origPositions[t] + Random.insideUnitSphere * mag;
            }

            yield return new WaitForSeconds(0.04f);

            foreach (var kvp in origPositions)
                if (kvp.Key != null) kvp.Key.position = kvp.Value;
        }
    }

    void TickSpike()
    {
        if (spikeTimeLeft > 0f)
        {
            spikeTimeLeft -= Time.unscaledDeltaTime;
            if (spikeTimeLeft <= 0f)
            {
                spikeTimeLeft = 0f;
                spikeIntensity = 0f;
                PushGlitchToShader(IntegrityRatio(), 0f);
            }
            return;
        }

        spikeTimer += Time.unscaledDeltaTime;
        if (spikeTimer < spikeInterval) return;
        spikeTimer = 0f;

        float prob = spikeProbability + (1f - IntegrityRatio()) * 0.4f;
        if (Random.value < prob)
            StartCoroutine(FireSpike());
    }

    IEnumerator FireSpike()
    {
        if (glitchSpikeSound != null) glitchSpikeSound.Play();

        spikeIntensity = Mathf.Clamp01(Random.Range(0.4f, 0.85f) * (1f + (1f - IntegrityRatio())));
        spikeTimeLeft = spikeDuration;

        PushGlitchToShader(IntegrityRatio(), spikeIntensity);
        FlickerLightsImmediate();

        yield return null;
    }

    void UpdateHUD()
    {
        float ratio = IntegrityRatio();
        if (integritySlider != null) integritySlider.value = ratio;
        if (sliderFill != null) sliderFill.color = Color.Lerp(criticalColor, healthyColor, ratio);
    }

    void TriggerEnding()
    {
        if (isEnding) return;
        isEnding = true;
        StartCoroutine(EndingSequence());
    }

    IEnumerator EndingSequence()
    {

        PushGlitchToShader(0f, 1f);
        UpdateLighting(1f);

        for (int i = 0; i < 8; i++)
        {
            if (glitchSpikeSound != null) glitchSpikeSound.Play();
            FlickerLightsImmediate();
            yield return new WaitForSecondsRealtime(0.05f);
        }

        if (fadeImage != null)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime * 1.5f;
                fadeImage.color = new Color(0, 0, 0, Mathf.Clamp01(t));
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.8f);
        }

        SceneManager.LoadScene(endSceneName);
    }

    void OnDestroy()
    {
        if (worldRenderers == null) return;
        for (int i = 0; i < worldRenderers.Length; i++)
        {
            if (worldRenderers[i] == null) continue;
            if (originalMaterials[i] != null)
                worldRenderers[i].sharedMaterial = originalMaterials[i];
        }
    }
}