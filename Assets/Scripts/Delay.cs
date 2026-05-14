using System.Collections;
using UnityEngine;

public class DelayedCollapsePlatform : MonoBehaviour
{
    public float delay = 5f;
    public Rigidbody rb;

    private bool triggered = false;

    private void Start()
    {
        rb.isKinematic = true; 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (triggered) return;

        if (collision.collider.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(Collapse());
        }
    }

    IEnumerator Collapse()
    {
        yield return new WaitForSeconds(delay);

        rb.isKinematic = false;
    }
}