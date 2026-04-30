using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T instance
    {
        get
        {
            if (_instance == null)
            {

                _instance = FindFirstObjectByType<T>();
                if (_instance != null)
                {
                    var go = new GameObject(" " + typeof(T));
                    _instance = go.AddComponent<T>();
                }
            }
            return _instance;
        }
    }
}