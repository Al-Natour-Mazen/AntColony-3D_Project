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
}
