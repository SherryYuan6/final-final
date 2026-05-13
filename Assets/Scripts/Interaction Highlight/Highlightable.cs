using UnityEngine;

namespace Highlighting
{
    [DisallowMultipleComponent]
    public class Highlightable : MonoBehaviour
    {
        [Header("Identity (for seen/taken)")]
        [Tooltip("Unique ID for saving progress. Leave empty to disable persistence.")]
        [SerializeField] private string persistentId;
        [SerializeField] private bool persistProgress = true;

        [Header("Type & progress")]
        [SerializeField] private HighlightInteractableType type = HighlightInteractableType.Normal;
        [SerializeField] private HighlightProgress progress = HighlightProgress.New;

        [Header("Availability")]
        [SerializeField] private HighlightAvailability availability = HighlightAvailability.Available;
        [SerializeField] private string lockedReason = "需要条件";

        [Header("Styles (shell tint; alpha * intensity = transparency — keep alpha modest for soft sci-fi glow)")]
        [SerializeField] private HighlightStyle normalAvailable = new HighlightStyle(new Color(0.42f, 0.62f, 0.78f, 0.26f), 1f, false);
        [SerializeField] private HighlightStyle clueNew = new HighlightStyle(new Color(0.38f, 0.74f, 0.76f, 0.36f), 1.05f, true);
        [SerializeField] private HighlightStyle unavailable = new HighlightStyle(new Color(0.82f, 0.56f, 0.42f, 0.30f), 0.9f, false);
        [SerializeField] private HighlightStyle completed = new HighlightStyle(new Color(0.52f, 0.56f, 0.62f, 0.16f), 1f, false);

        [Header("Controller")]
        [SerializeField] private HighlightController controller;

        public HighlightInteractableType Type => type;
        public HighlightProgress Progress => progress;
        public HighlightAvailability Availability => availability;
        public string LockedReason => lockedReason;

        private void Awake()
        {
            if (controller == null) controller = GetComponent<HighlightController>();
            if (controller == null) controller = gameObject.AddComponent<HighlightController>();

            if (persistProgress && !string.IsNullOrWhiteSpace(persistentId))
                progress = HighlightProgressStore.Load(persistentId, progress);
        }

        public void MarkSeen()
        {
            if (progress == HighlightProgress.New)
            {
                progress = HighlightProgress.Seen;
                SaveProgress();
            }
        }

        public void MarkTaken()
        {
            if (progress != HighlightProgress.Taken)
            {
                progress = HighlightProgress.Taken;
                SaveProgress();
            }
        }

        public void SetAvailability(HighlightAvailability availability, string reason = null)
        {
            this.availability = availability;
            if (!string.IsNullOrWhiteSpace(reason)) lockedReason = reason;
        }

        public void SetHighlight(float t01, bool strongFocus)
        {
            if (controller == null) return;

            var style = ResolveStyle(strongFocus);
            controller.SetTarget(t01, style.color, style.intensity, style.breathe);
        }

        public void ForceOff()
        {
            controller?.ForceOff();
        }

        private HighlightStyle ResolveStyle(bool strongFocus)
        {
            // Completed always weak
            if (progress == HighlightProgress.Taken || progress == HighlightProgress.Seen)
            {
                if (availability != HighlightAvailability.Available)
                    return new HighlightStyle(unavailable.color, Mathf.Min(unavailable.intensity, completed.intensity), false);
                return completed;
            }

            // Not completed yet
            if (availability != HighlightAvailability.Available)
                return unavailable;

            if (type == HighlightInteractableType.Clue)
            {
                // Only breathe when actually focused (avoid whole room pulsing)
                return new HighlightStyle(clueNew.color, clueNew.intensity, strongFocus && clueNew.breathe);
            }

            return normalAvailable;
        }

        private void SaveProgress()
        {
            if (!persistProgress) return;
            if (string.IsNullOrWhiteSpace(persistentId)) return;
            HighlightProgressStore.Save(persistentId, progress);
        }
    }
}

