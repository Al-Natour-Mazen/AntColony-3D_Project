using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class manages the carousel of cameras in the application.
/// </summary>
public class CameraCarousel : MonoBehaviour
{
    [Header("CameraCarousel Settings")]
    public Button camActivButton; // The button that activates the carousel
    public Sprite activeSprite; // The sprite that is displayed when the carousel is active
    public Sprite inactiveSprite; // The sprite that is displayed when the carousel is inactive
    public GameObject carousel; // The carousel object

    /// <summary>
    /// This method is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        // If the carousel object is not null, set it to inactive
        if (carousel != null)
        {
            carousel.SetActive(false);
        }

        // If the button is not null, set its image to the active sprite and add a listener for the click event
        if (camActivButton != null)
        {
            camActivButton.image.sprite = activeSprite;
            camActivButton.onClick.AddListener(() => ToggleCarousel());
        }
    }

    /// <summary>
    /// This method toggles the active state of the carousel and changes the button's image accordingly.
    /// </summary>
    public void ToggleCarousel()
    {
        // Check if the carousel is active
        bool isActive = carousel.activeSelf;

        // Toggle the active state of the carousel
        carousel.SetActive(!isActive);

        // If the carousel is now inactive, set the button's image to the inactive sprite
        if (!isActive)
        {
            camActivButton.image.sprite = inactiveSprite;
        }
        // If the carousel is now active, set the button's image to the active sprite
        else
        {
            camActivButton.image.sprite = activeSprite;
        }
    }
}
