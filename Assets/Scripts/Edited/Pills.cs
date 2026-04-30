using UnityEngine;

public class Pills : MonoBehaviour
{
    public float healAmount = 25f;
    public bool destroyOnUse = true;

    private static Pills currentPills;
    public bool playerInRange = false;
    void Update()
    {
        if (currentPills == this && playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            UsePill();
        }
    }

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

        if (destroyOnUse)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            currentPills = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (currentPills == this)
            {
                currentPills = null;
            }
        }
    }
}