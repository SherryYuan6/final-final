using UnityEngine;

public class LevelCollapse : MonoBehaviour
{
    public Rigidbody[] collapseObjects;
    public float explosionForce = 150;
    public float explosionRadius = 25;

    public void TriggerCollapse()
    {
        foreach (Rigidbody rb in collapseObjects)
        {
            if (rb == null) continue;
            rb.isKinematic = false;
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 0f, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * 10f, ForceMode.Impulse);
        }
    }
}