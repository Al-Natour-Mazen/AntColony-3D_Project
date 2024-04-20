using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleMenu : MonoBehaviour
{
    public GameObject menu1;
    public GameObject menu2;

    void Start()
    {
        showMenu1();
    }

    public void showMenu1()
    {
        if(menu1 != null && menu2 != null)
        {
            menu1.SetActive(true);
            menu2.SetActive(false);
        }
    }

    public void showMenu2()
    {
        if(menu1 != null && menu2 != null)
        {
            menu1.SetActive(false);
            menu2.SetActive(true);
        }
    }
}
