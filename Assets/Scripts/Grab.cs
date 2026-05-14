using UnityEngine;

public class GrabPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SimplePlatformerPlayer player =
            other.GetComponentInParent<SimplePlatformerPlayer>();

        if (player != null)
        {
            player.Grab(transform);
        }
    }
}