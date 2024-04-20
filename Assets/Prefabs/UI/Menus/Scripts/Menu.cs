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
        }
    }
}
