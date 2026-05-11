using System;

namespace Highlighting
{
    public enum HighlightInteractableType
    {
        Normal = 0,
        Clue = 1,
    }

    public enum HighlightProgress
    {
        New = 0,
        Seen = 1,
        Taken = 2,
    }

    public enum HighlightAvailability
    {
        Available = 0,
        Locked = 1,
        Disabled = 2,
    }

    [Serializable]
    public struct HighlightStyle
    {
        public UnityEngine.Color color;
        public float intensity;
        public bool breathe;

        public HighlightStyle(UnityEngine.Color color, float intensity, bool breathe)
        {
            this.color = color;
            this.intensity = intensity;
            this.breathe = breathe;
        }
    }
}

