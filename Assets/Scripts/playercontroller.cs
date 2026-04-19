using UnityEngine;

public class playercontroller : MonoBehaviour
{
    public CharacterController characterController;
    public float moveSpeed = 12;
    public float gravity = 9.8f;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    private bool isGrounded;
    private Vector3 velocity;
    private Transform feet;
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        feet = transform.Find("Feet");
    }

    private void Update()
    {
        Move();
        CheckisGrounded();
        ApplyGravity();
    }
    private void Move()
    {
        float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move);
    }

    private void CheckisGrounded()
    {
        isGrounded = Physics.CheckSphere(feet.position, groundCheckRadius, groundLayer);
    }

    private void ApplyGravity()
    {
        velocity += Vector3.down * gravity * Time.deltaTime;
        if (isGrounded)
            velocity = Vector3.zero;
        characterController.Move(velocity);
    }
}
