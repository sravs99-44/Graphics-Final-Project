using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Movement speed variables
    public float horizontalSpeed = 5f;
    public float verticalSpeed = 5f;
    public float zoomSpeed = 5f;

    // Reference to the camera component
    private Camera mainCamera;

    void Start()
    {
        // Get the main camera component
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Handle horizontal movement to right
        if (Input.GetKey(KeyCode.R))
        {
            // Move camera right relative to its current position
            transform.Translate(Vector3.right * horizontalSpeed * Time.deltaTime);
        }

        // Handle horizontal movement to left
        if (Input.GetKey(KeyCode.L))
        {
            // Move camera left relative to its current position
            transform.Translate(Vector3.left * horizontalSpeed * Time.deltaTime);
        }

        // Handle vertical movement up
        if (Input.GetKey(KeyCode.U))
        {
            // Move camera up relative to its current position
            transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime);
        }

        // Handle vertical movement down
        if (Input.GetKey(KeyCode.D))
        {
            // Move camera down relative to its current position
            transform.Translate(Vector3.down * verticalSpeed * Time.deltaTime);
        }

        // Handle zoom in
        if (Input.GetKey(KeyCode.Z))
        {
            // Move camera forward along its local Z-axis
            transform.Translate(Vector3.forward * zoomSpeed * Time.deltaTime);
        }

        // Handle zoom out
        if (Input.GetKey(KeyCode.O))
        {
            // Move camera backward along its local Z-axis
            transform.Translate(Vector3.back * zoomSpeed * Time.deltaTime);
        }
    }
}