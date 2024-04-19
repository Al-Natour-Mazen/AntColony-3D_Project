using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class SideMenu : MonoBehaviour
{
    [Header("Menu Settings")]
    public float widthMenu = 400.0f;

    [Header("Button Settings")]
    public Sprite buttonSprite;

    void OnValidate()
    {
        // To change the sprite of the button.
        Button button = gameObject.GetComponentInChildren<Button>();
        if(button != null && buttonSprite != null) 
        { 
            Image buttonImage = button.image;
            buttonImage.sprite = buttonSprite;
        }

        // To change the width of the menu.
        RectTransform rectT = gameObject.GetComponent<RectTransform>();
        EditorApplication.delayCall += () =>
        {
            if(rectT != null )
            {
                Vector2 currentSize = rectT.sizeDelta;
                currentSize.x = widthMenu;
                rectT.sizeDelta = currentSize;
            }
        };
        
    }
}
