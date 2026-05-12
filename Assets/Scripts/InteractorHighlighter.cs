using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Highlighting
{
    public class InteractorHighlighter : MonoBehaviour
    {
        [Header("Debug (remove after investigation)")]
        [SerializeField] private bool debugLogs;
        [Tooltip("0 = log focus ray every frame when debugLogs is on; >0 seconds between focus logs.")]
        [SerializeField] private float debugFocusLogEverySeconds = 0.15f;

        [Header("References")]
        [SerializeField] private Camera viewCamera;

        [Header("Distance")]
        [SerializeField] private float normalRange = 2.2f;
        [SerializeField] private float clueRange = 2.8f;
        [SerializeField, Range(0f, 1f)] private float nearbyWeakIntensity = 0.25f; 

        [Header("Raycast")]
        [SerializeField] private float raycastMaxDistance = 3.0f;
        [SerializeField] private LayerMask raycastMask = ~0;
        [Tooltip("Use Collide so rays hit Is Trigger volumes (recommended). Trigger colliders do not block CharacterController movement.")]
        [SerializeField] private QueryTriggerInteraction raycastTriggerInteraction = QueryTriggerInteraction.Collide;

        [Header("Scan (nearby weak highlight)")]
        [SerializeField] private bool enableNearbyWeakHighlight = true;
        [SerializeField] private float scanInterval = 0.12f;
        [SerializeField] private LayerMask scanMask = ~0;
        [SerializeField] private int scanMaxHits = 64;

        private readonly HashSet<Highlightable> nearby = new HashSet<Highlightable>();
        private readonly HashSet<Highlightable> previousNearbyScratch = new HashSet<Highlightable>();
        private Collider[] scanHits;
        private float nextScanAt;
        private Highlightable focused;
        private float nextFocusDebugAt;

        private RaycastHit lastFocusHit;
        private bool lastFocusHitValid;

        private void Awake()
        {
            if (viewCamera == null) viewCamera = Camera.main;
            scanHits = new Collider[Mathf.Max(8, scanMaxHits)];
        }

        private void Start()
        {
            if (!debugLogs) return;

            if (viewCamera == null)
            {
                Debug.LogWarning(
                    "[InteractorHighlighter] debugLogs=ON but viewCamera/Camera.main is NULL — Update() returns immediately; " +
                    "assign viewCamera on this component or tag your FPS camera as MainCamera.",
                    this);
            }
            else
            {
                Debug.Log(
                    $"[InteractorHighlighter] DEBUG ON — player=\"{gameObject.name}\" viewCamera=\"{viewCamera.name}\" " +
                    $"enableNearbyWeakHighlight={enableNearbyWeakHighlight}",
                    this);
            }
        }

        private void OnDisable()
        {
            foreach (var h in nearby)
                if (h != null) h.ForceOff();
            focused = null;
            lastFocusHitValid = false;
            nearby.Clear();
        }

        private void Update()
        {
            if (viewCamera == null) viewCamera = Camera.main;
            if (viewCamera == null) return;

            if (enableNearbyWeakHighlight && Time.time >= nextScanAt)
            {
                nextScanAt = Time.time + Mathf.Max(0.03f, scanInterval);
                RescanNearby();
            }

            UpdateFocus();
            ApplyHighlights();
        }

        private void RescanNearby()
        {
            previousNearbyScratch.Clear();
            foreach (var h in nearby)
                if (h != null) previousNearbyScratch.Add(h);

            nearby.Clear();

            float scanR = Mathf.Max(normalRange, clueRange);
            int count = Physics.OverlapSphereNonAlloc(transform.position, scanR, scanHits, scanMask, QueryTriggerInteraction.Collide);

            for (int i = 0; i < count; i++)
            {
                var c = scanHits[i];
                if (c == null) continue;

                var h = c.GetComponentInParent<Highlightable>();
                if (h == null) continue;

                nearby.Add(h);
            }

            if (debugLogs)
            {
                Debug.Log(
                    $"[InteractorHighlighter] RescanNearby OverlapSphere hits={count} nearbyHighlightables={nearby.Count} scanR={scanR} scanMask={scanMask.value} player={HierarchyPath(transform)}",
                    this);
                foreach (var h in nearby)
                {
                    if (h == null) continue;
                    Debug.Log(
                        $"[InteractorHighlighter]   nearby -> name=\"{h.gameObject.name}\" path=\"{HierarchyPath(h.transform)}\"",
                        h);
                }
            }

            foreach (var h in previousNearbyScratch)
            {
                if (h == null) continue;
                if (!nearby.Contains(h)) h.ForceOff();
            }
        }

        private void UpdateFocus()
        {
            focused = null;
            lastFocusHitValid = false;

            var origin = viewCamera.transform.position;
            var dir = viewCamera.transform.forward;

            bool shouldLogFocus = debugLogs &&
                                  (debugFocusLogEverySeconds <= 0f || Time.time >= nextFocusDebugAt);

            if (Physics.Raycast(origin, dir, out RaycastHit hit, raycastMaxDistance, raycastMask, raycastTriggerInteraction))
            {
                focused = hit.collider.GetComponentInParent<Highlightable>();
                if (focused != null)
                {
                    lastFocusHit = hit;
                    lastFocusHitValid = true;
                }

                if (shouldLogFocus)
                {
                    nextFocusDebugAt = Time.time + Mathf.Max(0f, debugFocusLogEverySeconds);
                    var hl = hit.collider.GetComponentInParent<Highlightable>();
                    Debug.Log(
                        $"[InteractorHighlighter] Raycast HIT collider=\"{hit.collider.name}\" distance={hit.distance:F3} " +
                        $"Highlightable={(hl == null ? "NULL" : $"OK '{hl.gameObject.name}'")} rayMax={raycastMaxDistance} triggers={raycastTriggerInteraction} raycastMask={raycastMask.value}",
                        hit.collider);
                }
            }
            else
            {
                if (shouldLogFocus)
                {
                    nextFocusDebugAt = Time.time + Mathf.Max(0f, debugFocusLogEverySeconds);
                    Debug.Log(
                        "[InteractorHighlighter] Raycast MISS — check: layer mask excludes geometry, " +
                        $"max distance too short, or something closer blocks the ray. rayMax={raycastMaxDistance} triggers={raycastTriggerInteraction} raycastMask={raycastMask.value} origin={origin} dir={dir}",
                        this);
                }
            }
        }

        private void ApplyHighlights()
        {
            foreach (var h in nearby)
            {
                if (h == null) continue;

                float range = h.Type == HighlightInteractableType.Clue ? clueRange : normalRange;
                float dPivot = Vector3.Distance(transform.position, h.transform.position);
                float dForRange = ComputeProximityDistance(h, dPivot);

                bool inRange = dForRange <= range;

                if (!inRange)
                {
                    if (debugLogs)
                        Debug.Log(
                            $"[InteractorHighlighter] ApplyHighlights name=\"{h.gameObject.name}\" dForRange={dForRange:F3} dPivot={dPivot:F3} range={range:F3} inRange=False focused==(h)={focused == h} SetHighlight t01=0",
                            h);

                    h.SetHighlight(0f, strongFocus: false);
                    continue;
                }

                bool isFocused = (focused == h);
                if (isFocused)
                {
                    if (debugLogs)
                        Debug.Log(
                            $"[InteractorHighlighter] ApplyHighlights name=\"{h.gameObject.name}\" dForRange={dForRange:F3} dPivot={dPivot:F3} range={range:F3} inRange=True focused==(h)=True SetHighlight t01=1",
                            h);

                    h.SetHighlight(1f, strongFocus: true);
                }
                else
                {
                    float t = enableNearbyWeakHighlight ? nearbyWeakIntensity : 0f;

   
                    if (debugLogs)
                        Debug.Log(
                            $"[InteractorHighlighter] ApplyHighlights name=\"{h.gameObject.name}\" dForRange={dForRange:F3} dPivot={dPivot:F3} range={range:F3} inRange=True focused==(h)=False SetHighlight t01={t:F3}",
                            h);

                    h.SetHighlight(t, strongFocus: false);
                }
            }
        }

  
        private float ComputeProximityDistance(Highlightable h, float dPivotFallback)
        {
            if (h == focused && lastFocusHitValid)
                return lastFocusHit.distance;

            return DistanceRootToClosestColliderSurface(transform.position, h);
        }

        private static float DistanceRootToClosestColliderSurface(Vector3 playerRootWorld, Highlightable h)
        {
            var cols = h.GetComponentsInChildren<Collider>(includeInactive: false);
            if (cols == null || cols.Length == 0)
                return Vector3.Distance(playerRootWorld, h.transform.position);

            float best = float.PositiveInfinity;
            for (int i = 0; i < cols.Length; i++)
            {
                var c = cols[i];
                if (c == null || !c.enabled) continue;
                if (c.GetComponent<HighlightShell>() != null) continue;

                var closest = c.ClosestPoint(playerRootWorld);
                float d = Vector3.Distance(playerRootWorld, closest);
                if (d < best) best = d;
            }

            return float.IsPositiveInfinity(best)
                ? Vector3.Distance(playerRootWorld, h.transform.position)
                : best;
        }

        private static string HierarchyPath(Transform t)
        {
            if (t == null) return "(null)";
            var sb = new StringBuilder(128);
            while (t != null)
            {
                if (sb.Length > 0) sb.Insert(0, '/');
                sb.Insert(0, t.name);
                t = t.parent;
            }

            return sb.ToString();
        }
    }
}

