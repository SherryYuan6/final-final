using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Highlighting
{
    /// <summary>
    /// Drives highlight shell mesh renderers: shared Unlit transparent material + per-renderer MPB tint.
    /// Does not modify original meshes or materials.
    /// </summary>
    [DisallowMultipleComponent]
    public class HighlightController : MonoBehaviour
    {
        [Header("Shell material (one shared asset for the project)")]
        [Tooltip("Assign Assets/Highlighting/Materials/HighlightShell (or your instance of the same shader).")]
        [SerializeField] private Material shellMaterial;

        [Header("Shell setup")]
        [Tooltip("If true, creates a child __HighlightShell under each MeshFilter+MeshRenderer (same mesh, scaled up). Turn off if you place shells manually.")]
        [SerializeField] private bool autoCreateShells = true;
        [SerializeField] private float shellScale = 1.03f;
        [SerializeField] private string shellObjectName = "__HighlightShell";

        [Header("Smoothing")]
        [SerializeField] private float fadeInTime = 0.12f;
        [SerializeField] private float fadeOutTime = 0.18f;

        [Header("Breathing (subtle)")]
        [SerializeField] private float breatheHz = 0.7f;
        [SerializeField, Range(0f, 0.5f)] private float breatheAmplitude = 0.2f;

        // DEBUG: toggle off / delete this block after fixing "shell not visible"
        [Header("Debug (remove after investigation)")]
        [SerializeField] private bool debugLogs;
        [SerializeField, Tooltip("Log ApplyShells when |alpha delta| exceeds this (visible flip always logs).")]
        private float debugShellAlphaEpsilon = 0.02f;

        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");

        private readonly List<MeshRenderer> shellRenderers = new List<MeshRenderer>();
        private MaterialPropertyBlock mpb;

        private float current;
        private float target;
        private Color tintColor = Color.white;
        private float intensity = 0f;
        private bool breathe;

        private bool debugLastShellVisible;
        private float debugLastShellAlpha = -1f;
        private bool debugShellStateInitialized;

        private void Awake()
        {
            mpb = new MaterialPropertyBlock();
            shellRenderers.Clear();

            if (autoCreateShells)
            {
                if (shellMaterial == null)
                {
                    Debug.LogError(
                        "[HighlightController] Assign the shared highlight shell Material (e.g. HighlightShell.mat).",
                        this);
                }
                else
                {
                    AutoCreateOrReuseShells();
                }
            }
            else
            {
                CollectManualShells();
            }

            ForceOffImmediate();

            debugShellStateInitialized = false;
            debugLastShellVisible = false;
            debugLastShellAlpha = -1f;

            // DEBUG: remove after investigation — one-shot proof HighlightController + debug flag are alive
            if (debugLogs)
            {
                Debug.Log(
                    $"[HighlightController] DEBUG ON — shells={shellRenderers.Count} autoCreateShells={autoCreateShells} " +
                    $"shellMaterial={(shellMaterial == null ? "NULL" : shellMaterial.name)} go=\"{gameObject.name}\"",
                    this);
            }
        }

        private void OnDisable()
        {
            ForceOffImmediate();
        }

        public void SetTarget(float t01, Color color, float intensity, bool breathe)
        {
            target = Mathf.Clamp01(t01);
            tintColor = color;
            this.intensity = Mathf.Max(0f, intensity);
            this.breathe = breathe;
        }

        public void ForceOff()
        {
            target = 0f;
        }

        private void ForceOffImmediate()
        {
            current = 0f;
            target = 0f;
            ApplyShells(Color.clear);
        }

        private void Update()
        {
            var dt = Time.deltaTime;
            var time = target > current ? Mathf.Max(0.0001f, fadeInTime) : Mathf.Max(0.0001f, fadeOutTime);
            current = Mathf.MoveTowards(current, target, dt / time);

            float breatheMul = 1f;
            if (breathe && current > 0.001f)
                breatheMul = 1f + breatheAmplitude * Mathf.Sin(Time.time * Mathf.PI * 2f * breatheHz);

            var c = tintColor * (intensity * current * breatheMul);
            ApplyShells(c);
        }

        private void ApplyShells(Color baseColor)
        {
            bool visible = baseColor.a > 0.0001f;

            // DEBUG: remove after investigation — log when shell visibility toggles or alpha jumps noticeably
            if (debugLogs)
            {
                bool visibleChanged = !debugShellStateInitialized || visible != debugLastShellVisible;
                bool alphaJump = !debugShellStateInitialized ||
                                 Mathf.Abs(baseColor.a - debugLastShellAlpha) > Mathf.Max(0f, debugShellAlphaEpsilon);

                if (!debugShellStateInitialized || visibleChanged || alphaJump)
                {
                    Debug.Log(
                        $"[HighlightController] ApplyShells shellRenderers.Count={shellRenderers.Count} visible={visible} " +
                        $"baseColor=RGBA({baseColor.r:F3},{baseColor.g:F3},{baseColor.b:F3},{baseColor.a:F4}) go=\"{gameObject.name}\" path=\"{HierarchyPath(transform)}\"",
                        this);
                }

                debugShellStateInitialized = true;
                debugLastShellVisible = visible;
                debugLastShellAlpha = baseColor.a;
            }

            for (int i = 0; i < shellRenderers.Count; i++)
            {
                var mr = shellRenderers[i];
                if (mr == null) continue;

                mr.enabled = visible;
                if (!visible) continue;

                mr.GetPropertyBlock(mpb);
                mpb.SetColor(BaseColorId, baseColor);
                mr.SetPropertyBlock(mpb);
            }
        }

        private static string HierarchyPath(Transform t)
        {
            if (t == null) return "(null)";
            var sb = new System.Text.StringBuilder(128);
            while (t != null)
            {
                if (sb.Length > 0) sb.Insert(0, '/');
                sb.Insert(0, t.name);
                t = t.parent;
            }

            return sb.ToString();
        }

        private void AutoCreateOrReuseShells()
        {
            var meshRenderers = GetComponentsInChildren<MeshRenderer>(includeInactive: true);
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                var baseMr = meshRenderers[i];
                if (baseMr == null) continue;
                if (baseMr.GetComponent<HighlightShell>() != null) continue;

                var mf = baseMr.GetComponent<MeshFilter>();
                if (mf == null || mf.sharedMesh == null) continue;

                var t = baseMr.transform.Find(shellObjectName);
                if (t != null)
                {
                    var mark = t.GetComponent<HighlightShell>();
                    var existingMr = t.GetComponent<MeshRenderer>();
                    if (mark != null && existingMr != null)
                    {
                        EnsureShellUsesSharedMaterial(existingMr, baseMr);
                        shellRenderers.Add(existingMr);
                        continue;
                    }
                }

                CreateShellFor(baseMr, mf);
            }
        }

        private void CreateShellFor(MeshRenderer baseMr, MeshFilter mf)
        {
            var go = new GameObject(shellObjectName);
            go.transform.SetParent(baseMr.transform, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one * shellScale;

            var shellMf = go.AddComponent<MeshFilter>();
            shellMf.sharedMesh = mf.sharedMesh;

            var shellMr = go.AddComponent<MeshRenderer>();
            int n = Mathf.Max(1, baseMr.sharedMaterials.Length);
            var mats = new Material[n];
            for (int j = 0; j < n; j++) mats[j] = shellMaterial;
            shellMr.sharedMaterials = mats;
            shellMr.shadowCastingMode = ShadowCastingMode.Off;
            shellMr.receiveShadows = false;
            shellMr.allowOcclusionWhenDynamic = false;
            shellMr.enabled = false;

            go.AddComponent<HighlightShell>();
            shellRenderers.Add(shellMr);
        }

        private void EnsureShellUsesSharedMaterial(MeshRenderer shellMr, MeshRenderer baseMr)
        {
            if (shellMaterial == null) return;

            int n = Mathf.Max(1, baseMr.sharedMaterials.Length);
            var mats = shellMr.sharedMaterials;
            if (mats == null || mats.Length != n)
            {
                mats = new Material[n];
                for (int i = 0; i < n; i++) mats[i] = shellMaterial;
                shellMr.sharedMaterials = mats;
                return;
            }

            for (int i = 0; i < mats.Length; i++)
            {
                if (mats[i] != shellMaterial) mats[i] = shellMaterial;
            }

            shellMr.sharedMaterials = mats;
        }

        private void CollectManualShells()
        {
            var shells = GetComponentsInChildren<HighlightShell>(includeInactive: true);
            for (int i = 0; i < shells.Length; i++)
            {
                var mr = shells[i].GetComponent<MeshRenderer>();
                if (mr != null) shellRenderers.Add(mr);
            }
        }
    }
}
