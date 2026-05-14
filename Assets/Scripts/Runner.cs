using UnityEngine;

public class SimplePlatformerPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 7f;
    public Rigidbody rb;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Grab")]
    public bool isGrabbing;
    public Transform grabPoint;

    bool isGrounded;

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");

        if (isGrabbing) return;

        Vector3 vel = rb.linearVelocity;
        vel.x = x * moveSpeed;
        rb.linearVelocity = vel;
    }

    void Jump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isGrabbing)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void Grab(Transform ledge)
    {
        isGrabbing = true;
        rb.linearVelocity = Vector3.zero;
        rb.useGravity = false;

        transform.position = ledge.position;
    }

    public void Release()
    {
        isGrabbing = false;
        rb.useGravity = true;
    }
}