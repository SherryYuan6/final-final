using UnityEngine;

public class Pills : MonoBehaviour
{
    public float healAmount = 25f;

    public void UsePill()
    {
        if (Decay.Instance != null)
        {
            Decay.Instance.AddCognitive(healAmount);
            Debug.Log("Pill used. Sanity +" + healAmount);
        }
        else
        {
            Debug.LogWarning("Decay.Instance is null");
        }
    }
}