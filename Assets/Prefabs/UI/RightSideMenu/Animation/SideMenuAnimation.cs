using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMenuAnimation : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    public void ShowHideMenu()
    {
        if(animator != null )
        {
            bool isOpen = animator.GetBool("show");
            animator.SetBool("show", !isOpen);
        }
    }
}
