using UnityEngine;

namespace Highlighting
{
    /// <summary>
    /// Marks a mesh shell used only for outline/highlight rendering. Base mesh scan skips these.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class HighlightShell : MonoBehaviour
    {
    }
}
