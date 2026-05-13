using UnityEngine;

public class ChipIntegrity : MonoBehaviour
{
    public enum ZoneType { Drain, Restore }

    public ZoneType zoneType = ZoneType.Drain;
    public float ratePerSecond = 5f;
    public GameObject indicator;

    private bool playerInside = false;

    void Update()
    {
        if (!playerInside || ChipManager.Instance == null) return;
        float delta = ratePerSecond * Time.deltaTime;
        if (zoneType == ZoneType.Drain) ChipManager.Instance.LoseIntegrity(delta);
        else ChipManager.Instance.AddIntegrity(delta);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = true;
        if (indicator != null) indicator.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
        if (indicator != null) indicator.SetActive(false);
    }
}