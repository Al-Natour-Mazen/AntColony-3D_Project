using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls different camera views and camera movement around a colony.
/// </summary>
public class ColonyCamera : MonoBehaviour
{
    [Header("Simulation Settings")]
    public SimulationController simulation; // Reference to the simulation controller
    public GameObject[] objectToMask; // Objects to mask when colony camera is active
    public Transform colony; // Reference to the colony's transform
    public float colonyRadius; // Radius of the colony
    public GameObject joystick; // Reference to the joystick GameObject

    [Header("Camera References")]
    public Camera mainCamera; // Reference to the main camera
    public Camera colonyCamera; // Reference to the colony camera
    public Camera FPSCamera; // Reference to the first-person shooter (FPS) camera

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    void Update()
    {
       if (colony != null)
        {
            // Calculate the distance between the camera and the colony
            float distance = Vector3.Distance(transform.position, colony.position);

            // Limit the distance to the colony
            if (distance > colonyRadius)
            {
                // Get the direction from the colony to the camera
                Vector3 directionToColony = (colony.position - transform.position).normalized;

                // Move the camera towards the colony up to the distance limit
                transform.position = colony.position - directionToColony * (colonyRadius + 10);
            }

            // Make the camera look at the colony
            transform.LookAt(colony);
        }
    }

    /// <summary>
    /// Switches to the main camera view.
    /// </summary>
    public void changeToMainCamera()
    {
        if(mainCamera != null && colonyCamera != null && FPSCamera != null)
        {
            mainCamera.enabled = true;
            colonyCamera.enabled = false;
            FPSCamera.enabled = false;
            joystick.SetActive(false);
        }
    }

    /// <summary>
    /// Switches to the colony camera view.
    /// </summary>
    public void changeToColonyCamera()
    {
        if(mainCamera != null && colonyCamera != null && FPSCamera != null)
        {
            mainCamera.enabled = false;
            colonyCamera.enabled = true;
            FPSCamera.enabled = false;
            joystick.SetActive(true);
        }
    }

    /// <summary>
    /// Switches to the first-person camera view.
    /// </summary>
    public void changeToFPSCamera()
    {
        if(mainCamera != null && colonyCamera != null && FPSCamera != null)
        {
            mainCamera.enabled = false;
            colonyCamera.enabled = false;
            FPSCamera.enabled = true;
            joystick.SetActive(false);
        }
    }
}