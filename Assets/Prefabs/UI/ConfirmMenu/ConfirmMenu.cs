using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages a confirmation menu for prompting user actions.
/// </summary>
public class ConfirmMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    public string sceneToGo; // Name of the scene to navigate to

    [Header("Text Settings")]
    [TextArea(3,10)]
    public string confirmText; // Text displayed for confirmation message
    public string confirmButtonText; // Text displayed on the confirm button
    public string cancelButtonText; // Text displayed on the cancel button

    public Text ConfirmText; // Text component displaying the confirmation message
    public Text ConfirmBText; // Text component displaying the confirm button text
    public Text CancelBText; // Text component displaying the cancel button text

    public bool displayConfirmBox = true; // Flag indicating whether to display the confirmation box

    void Start()
    {
        HideConfirmBox();
        setNewTexts();
    }

    void OnValidate()
    {
        setNewTexts();
    }

    private void setNewTexts()
    {
        // Update UI texts with new values
        if (ConfirmText != null)
        {
            ConfirmText.text = confirmText;
        }
        if (ConfirmBText != null)
        {
            ConfirmBText.text = confirmButtonText;
        }
        if (CancelBText != null)
        {
            CancelBText.text = cancelButtonText;
        }
    }

    /// <summary>
    /// Changes the scene to the specified sceneToGo.
    /// </summary>
    public void ChangeScene()
    {
        if (sceneToGo != "" && sceneToGo != null)
        {
            SceneManager.LoadSceneAsync(sceneToGo);
        }
    }

    /// <summary>
    /// Displays the confirmation box or changes scene directly if displayConfirmBox is false.
    /// </summary>
    public void DisplayConfirmBox()
    {
        if (displayConfirmBox)
        {
            SetActiveConfirmBox(true);
        }
        else
        {
            ChangeScene();
        }
    }

    /// <summary>
    /// Hides the confirmation box.
    /// </summary>
    public void HideConfirmBox()
    {
        SetActiveConfirmBox(false);
    }

    private void SetActiveConfirmBox(bool active)
    {
        // Set the active state of the confirmation box GameObject
        gameObject.SetActive(active);
    }
}
