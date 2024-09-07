using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourDoor: MonoBehaviour
{
    private Animator animator;
    public bool isOpen { get; private set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
        isOpen = false;
    }

    void Update()
    {

    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            animator.SetTrigger("Open");
            isOpen = true;
            Debug.Log("Opened door: " + gameObject.name);
        }
        else
        {
            animator.SetTrigger("Close");
            isOpen = false;
            Debug.Log("Closed door: " + gameObject.name);
        }
    }
}
