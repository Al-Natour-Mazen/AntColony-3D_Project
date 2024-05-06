using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FPSCamera : MonoBehaviour
{
    public float vitesseDeplacement = 30f;
    public float vitesseRotation = 80f;
    public Transform player;
    public Camera FPScamera;
    public float mouseSensitivity = 2f;
    float cameraVerticalRotation = 0f;

    private bool cameraMovement = false;

    // Update is called once per frame
    void Update()
    {
        cameraMouvement();
        cursor();
    }

    void cameraMouvement()
    {
        // Déplacement vers l'avant avec la touche W -> Z
        if (Input.GetKey(KeyCode.W))
        {
            player.Translate(Vector3.forward * vitesseDeplacement * Time.deltaTime);
        }

        // Déplacement vers la gauche avec la touche Q -> A
        if (Input.GetKey(KeyCode.A))
        {
            player.Translate(Vector3.left * vitesseDeplacement * Time.deltaTime);
        }

        // Déplacement vers l'arrière avec la touche S
        if (Input.GetKey(KeyCode.S))
        {
            player.Translate(Vector3.back * vitesseDeplacement * Time.deltaTime);
        }

        // Déplacement vers la droite avec la touche D
        if (Input.GetKey(KeyCode.D))
        {
            player.Translate(Vector3.right * vitesseDeplacement * Time.deltaTime);
        }

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
