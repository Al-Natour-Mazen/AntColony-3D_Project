using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColonyCamera : MonoBehaviour
{
    public SimulationController simulation;
    public GameObject[] objectToMask;
    public Transform colony;
    public float colonyRadius;
    public GameObject joystick;

    public Camera mainCamera;
    public Camera colonyCamera;

    void Update()
    {
       if (colony != null)
        {
            // Calculer la distance entre la caméra et la colonie
            float distance = Vector3.Distance(transform.position, colony.position);

            // Limiter la distance à la colonie
            if (distance > colonyRadius)
            {
                // Obtenir la direction de la colonie vers la caméra
                Vector3 directionToColony = (colony.position - transform.position).normalized;

                // Déplacer la caméra vers la colonie jusqu'à la limite de la distance
                transform.position = colony.position - directionToColony * (colonyRadius + 10);
            }


            transform.LookAt(colony);
        }
    }

    public void changeToMainCamera()
    {
        if(mainCamera != null && colonyCamera != null)
        {
            mainCamera.enabled = true;
            colonyCamera.enabled = false;
            joystick.SetActive(false);
        }
    }

    public void changeToColonyCamera()
    {
        if(mainCamera != null && colonyCamera != null)
        {
            mainCamera.enabled = false;
            colonyCamera.enabled = true;
            joystick.SetActive(true);
        }
    }

}
