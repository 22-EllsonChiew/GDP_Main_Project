using UnityEngine;

public class WindowController : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) // Replace with your preferred key
        {
            TogglePanels();
        }
    }

    public void TogglePanels()
    {
        if (isOpen)
        {
            animator.SetTrigger("CloseAllPanels"); // Trigger for closing all panels
            isOpen = false;
        }
        else
        {
            animator.SetTrigger("OpenAllPanels"); // Trigger for opening all panels
            isOpen = true;
        }
    }
}
