using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Menu Settings")]
    public string title;


    void OnValidate()
    {
        Text text = gameObject.GetComponentInChildren<Text>();
        if(text != null && title != null) 
        { 
            text.text = title;
        }
    }

    public void EnableButton()
    {
        ButtonClick(true);
    }

    public void DisableButton()
    {
        ButtonClick(false);
    }

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
