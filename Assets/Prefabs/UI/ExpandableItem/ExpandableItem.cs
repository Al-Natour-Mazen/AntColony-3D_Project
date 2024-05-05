using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandableItem : MonoBehaviour
{
    [Header("Button Settings")]
    [Tooltip("The Button Item")]
    public Button itemButton;

    [Tooltip("The Background Image of the Item.")]
    public Sprite backgroundImage;

    [Tooltip("The scale factor for height enlargement.")]
    public float enlargementFactor = 1.5f;

    private bool isEnlarged = false;

    [Header("Text Settings")]
    [Tooltip("The Title for each textTitle Component.")]
    public string title;

    [Tooltip("The TitleBis for each textTitleBis Component.")]
    public string titleBis;

    [Tooltip("The Description for each textDescription Component.")]
    public string description;

    [Tooltip("Reference to the UI Text component displaying the Title text.")]
    public Text textTitle;

    [Tooltip("Reference to the UI Text component displaying the TitleBis text.")]
    public Text textTitleBis;

    [Tooltip("Reference to the UI Text component displaying the TitleDescription text.")]
    public Text textDescription;

    // Called in the editor when a value is changed.
    void OnValidate()
    {
        // Update UI text components if they are assigned and the corresponding string values are not empty
        if (textTitle != null && title != "")
        {
            textTitle.text = title;
        }
        if (textTitleBis != null && titleBis != "")
        {
            textTitleBis.text = titleBis;
        }

        if (textDescription != null && description != "")
        {
            textDescription.text = description;
        }
    }

    void Start()
    {
        // Assign background image and add click listener to the button
        if (itemButton != null)
        {
            itemButton.GetComponent<Image>().sprite = backgroundImage;
            itemButton.onClick.AddListener(ToggleButtonAndDescriptionSize);
        }
    }

    // Toggle the size of the button and description text
    void ToggleButtonAndDescriptionSize()
    {
        RectTransform buttonRect = itemButton.GetComponent<RectTransform>();
        RectTransform descriptionRect = textDescription.GetComponent<RectTransform>();

        if (isEnlarged)
        {
            // Shrink button and description text to original size
            buttonRect.sizeDelta = new Vector2(buttonRect.sizeDelta.x, buttonRect.sizeDelta.y / enlargementFactor);
            descriptionRect.sizeDelta = new Vector2(descriptionRect.sizeDelta.x, descriptionRect.sizeDelta.y / enlargementFactor);
        }
        else
        {
            // Enlarge button and description text
            buttonRect.sizeDelta = new Vector2(buttonRect.sizeDelta.x, buttonRect.sizeDelta.y * enlargementFactor);
            descriptionRect.sizeDelta = new Vector2(descriptionRect.sizeDelta.x, descriptionRect.sizeDelta.y * enlargementFactor);
        }

        // Toggle the state
        isEnlarged = !isEnlarged;
    }

    // Getters and setters for title, titleBis, and description
    public string GetTitle()
    {
        return title;
    }

    public void SetTitle(string newTitle)
    {
        title = newTitle;
        textTitle.text = newTitle;
    }

    public string GetTitleBis()
    {
        return titleBis;
    }

    public void SetTitleBis(string newTitleBis)
    {
        titleBis = newTitleBis;
        textTitleBis.text = newTitleBis;
    }

    public string GetDescription()
    {
        return description;
    }

    public void SetDescription(string newDescription)
    {
        description = newDescription;
        textDescription.text = newDescription;
    }
}
