using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main class responsible for managing the menu functionality.
/// </summary>
public class Menu : MonoBehaviour
{
    [Header("Menu Settings")]
    public string title; // The title of the menu.

    /// <summary>
    /// This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only).
    /// </summary>
    void OnValidate()
    {
        // Update the title text of the menu.
        Text text = gameObject.GetComponentInChildren<Text>();
        if(text != null && title != null) 
        { 
            text.text = title;
        }
    }

    /// <summary>
    /// Enables the button associated with this menu.
    /// </summary>
    public void EnableButton()
    {
        ButtonClick(true);
    }

    /// <summary>
    /// Disables the button associated with this menu.
    /// </summary>
    public void DisableButton()
    {
        ButtonClick(false);
    }

    /// <summary>
    /// Handles the click event of the button associated with this menu.
    /// </summary>
    /// <param name="state">The state to set the button to (enabled or disabled).</param>
    private void ButtonClick(bool state)
    {
        Button button = gameObject.GetComponentInChildren<Button>();
        if(button != null)
        {
            button.enabled = state;
            // Get the Image component of the button
            Image image = button.GetComponent<Image>();

            if (!button.enabled)
            {
                // Change the color of the image to #5C5050 with alpha 112 when the button is not enabled
                image.color = new Color32(92, 80, 80, 112);
            }
            else
            {
                // Change the color of the image back to normal when the button is enabled
                image.color = Color.white;
            }
        }
    }
}