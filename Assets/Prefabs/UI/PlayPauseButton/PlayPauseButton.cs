using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script toggles between play and pause states when attached to a UI button.
/// </summary>
public class PlayPauseButton : MonoBehaviour
{
    public Sprite playImage; // The sprite for the play state.
    public Sprite pauseImage; // The sprite for the pause state.
    private bool isPlaying = false; // Flag indicating whether the media is playing.
    private Image image; // Reference to the Image component of the button.
    private Button btn; // Reference to the Button component.

    void Start()
    {
        // Get references to the Image and Button components attached to this GameObject.
        btn = GetComponent<Button>();
        image = GetComponent<Image>();

        // Set the default sprite to the play image.
        image.sprite = playImage;

        // Add a listener for button clicks.
        btn.onClick.AddListener(OnButtonClick);
    }

    /// <summary>
    /// Handles the button click event.
    /// </summary>
    private void OnButtonClick()
    {
        // Toggle the play state.
        isPlaying = !isPlaying;

        // Update the button's sprite based on the current play state.
        if (isPlaying)
        {
            image.sprite = pauseImage; // Change to pause image.
        }
        else
        {
            image.sprite = playImage; // Change to play image.
        }
    }

    /// <summary>
    /// Returns whether the media is currently playing.
    /// </summary>
    /// <returns>True if the media is playing, false otherwise.</returns>
    public bool IsPlaying()
    {
        return isPlaying;
    }

    /// <summary>
    /// Changes the state of a button when called.
    /// </summary>
    public void ChangeButtonPlayingState()
    {
        OnButtonClick();
    }

    /// <summary>
    /// Enables or disables the button and changes the button image color to grey when disabled.
    /// <param name="state">The state of the button. True to enable the button, false to disable it.</param>
    /// </summary>
    public void EnableDiableButton(bool state)
    {
        btn.enabled = state;
        if (!btn.enabled)
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
