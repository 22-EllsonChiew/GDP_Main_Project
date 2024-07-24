using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour
{
    public Animator animator;
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Replace KeyCode.E with the key you want to use
        {
            if (!isOpen)
            {
                animator.SetTrigger("Open");
                isOpen = true;
            }
            else
            {
                animator.SetTrigger("Close");
                isOpen = false;
            }
        }
    }
}
