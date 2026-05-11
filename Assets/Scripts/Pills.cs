using UnityEngine;

public class Pills : MonoBehaviour
{
    public float healAmount = 25f;

    public void UsePill()
    {
        if (ChipManager.Instance != null)
        {
            ChipManager.Instance.AddCognitive(healAmount);
        //    Debug.Log("Pill used. Sanity +" + healAmount);
        //}
        //else
        //{
        //    Debug.LogWarning("Decay.Instance is null");
        }
    }
}