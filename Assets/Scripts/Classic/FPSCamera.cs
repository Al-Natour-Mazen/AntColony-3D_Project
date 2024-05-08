using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Controls the first-person camera movement and input handling.
/// </summary>
public class FPSCamera : MonoBehaviour
{
    [Header("Movement Settings")]
    public float vitesseDeplacement = 30f; // Movement speed
    public float vitesseRotation = 80f; // Rotation speed
    public Transform player; // Player's transform
    public Camera FPScamera; // First-person camera
    public float mouseSensitivity = 2f; // Mouse sensitivity

    [Header("Boundary Settings")]
    public float minX = 0f; // Minimum X boundary
    public float maxX = 10f; // Maximum X boundary
    public float minZ = 0f; // Minimum Z boundary
    public float maxZ = 10f; // Maximum Z boundary

    private bool cameraMovement = false; // Flag indicating if camera movement is enabled
    private float cameraVerticalRotation = 90f; // Vertical rotation of the camera

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
        cameraMouvement();
        cursor();
    }

    /// <summary>
    /// Handles camera movement based on player input.
    /// </summary>
    void cameraMouvement()
    {
        Vector3 moveDirection = Vector3.zero;

        // Move forward with W key
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector3.forward;
        }

        // Move left with A key
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector3.left;
        }

        // Move backward with S key
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += Vector3.back;
        }

        // Move right with D key
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right;
        }

        // Normalize the movement vector to avoid faster diagonal movements
        moveDirection = moveDirection.normalized;

        // Apply movement
        player.Translate(moveDirection * vitesseDeplacement * Time.deltaTime);

        // Clamp camera within the rectangular perimeter
        Vector3 clampedPosition = player.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minZ, maxZ);
        player.position = clampedPosition;

        if(cameraMovement)
        {
            float inputX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float inputY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            cameraVerticalRotation -= inputY;
            cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
            transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

            player.Rotate(Vector3.up * inputX);
        } 
    }

    /// <summary>
    /// Handles cursor visibility and locking based on camera state.
    /// </summary>
    void cursor()
    {
        if (FPScamera.enabled)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                cameraMovement = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                cameraMovement = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        } 
    }
}
