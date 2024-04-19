using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenu : MonoBehaviour
{
    [Header("Button Settings")]
    public Sprite buttonSprite;


    void OnValidate()
    {
        Button button = gameObject.GetComponentInChildren<Button>();
        if(button != null && buttonSprite != null) 
        { 
            Image buttonImage = button.image;
            buttonImage.sprite = buttonSprite;
            Debug.Log("spriteChanged");
        }
    }
}
