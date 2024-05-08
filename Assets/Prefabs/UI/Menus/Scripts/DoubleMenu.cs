using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for managing two menus.
/// </summary>
public class DoubleMenu : MonoBehaviour
{
    [Header("Menu References")]
    public GameObject menu1; // Reference to the first menu GameObject.
    public GameObject menu2; // Reference to the second menu GameObject.

    /// <summary>
    /// Called on the frame when a script is enabled just before any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        // Show the first menu when the script starts.
        showMenu1();
    }

    /// <summary>
    /// Shows the first menu and hides the second menu.
    /// </summary>
    public void showMenu1()
    {
        if(menu1 != null && menu2 != null)
        {
            menu1.SetActive(true);
            menu2.SetActive(false);
        }
    }

    /// <summary>
    /// Shows the second menu and hides the first menu.
    /// </summary>
    public void showMenu2()
    {
        if(menu1 != null && menu2 != null)
        {
            menu1.SetActive(false);
            menu2.SetActive(true);
        }
    }
}