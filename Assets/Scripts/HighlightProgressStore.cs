using UnityEngine;

namespace Highlighting
{
    public static class HighlightProgressStore
    {
        private const string Prefix = "hl.progress.";

        public static HighlightProgress Load(string id, HighlightProgress fallback)
        {
            if (string.IsNullOrWhiteSpace(id)) return fallback;
            return (HighlightProgress)PlayerPrefs.GetInt(Prefix + id, (int)fallback);
        }

        public static void Save(string id, HighlightProgress progress)
        {
            if (string.IsNullOrWhiteSpace(id)) return;
            PlayerPrefs.SetInt(Prefix + id, (int)progress);
        }
    }
}

