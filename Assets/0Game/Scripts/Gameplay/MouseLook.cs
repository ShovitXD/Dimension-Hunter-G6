using Photon.Pun;
using UnityEngine;

public class MouseLook : MonoBehaviourPunCallbacks
{
    public float mouseSensitivity = 100f; // Mouse sensitivity
    public Transform playerBody; // Reference to the capsule (player body)
    public Transform playerCamera; // Reference to the camera (child of the capsule)

    private float xRotation = 0f; // Tracks vertical camera rotation

    void Start()
    {
        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleCameraRotation();
    }

    void HandleCameraRotation()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate the player body horizontally
        playerBody.Rotate(Vector3.up * mouseX);

        // Rotate the camera vertically
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}