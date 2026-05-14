using UnityEngine;

public class Pills : MonoBehaviour
{
    public float healAmount = 50f;

    public void UsePill()
    {
        if (ChipManager.Instance == null) return;

        ChipManager.Instance.AddCognitive(healAmount);
    }
}