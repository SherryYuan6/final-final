using UnityEngine;

public class cameracontroller : MonoBehaviour
{
    public float mouseSensitivity = 90;

    public float xRotation;
    public float yRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xRotation = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible= false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y")*mouseSensitivity*Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.parent.Rotate(Vector3.up*mouseX);

        transform.localRotation = Quaternion.Euler(xRotation,0,0);
    }
}
