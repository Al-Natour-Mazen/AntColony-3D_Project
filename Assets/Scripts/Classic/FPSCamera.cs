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

    public float minX = 0f;
    public float maxX = 10f;
    public float minZ = 0f;
    public float maxZ = 10f;

    private bool cameraMovement = false;
    private float cameraVerticalRotation = 90f;

    // Update is called once per frame
    void Update()
    {
        cameraMouvement();
        cursor();
    }

    void cameraMouvement()
    {
        Vector3 moveDirection = Vector3.zero;

        // Déplacement vers l'avant avec la touche W -> Z
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector3.forward;
        }

        // Déplacement vers la gauche avec la touche Q -> A
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector3.left;
        }

        // Déplacement vers l'arrière avec la touche S
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += Vector3.back;
        }

        // Déplacement vers la droite avec la touche D
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right;
        }

        // Normalise le vecteur de déplacement pour éviter les mouvements diagonaux plus rapides
        moveDirection = moveDirection.normalized;

        // Appliquer le déplacement
        player.Translate(moveDirection * vitesseDeplacement * Time.deltaTime);

        // Limiter la caméra dans le périmètre rectangulaire
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