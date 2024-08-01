using UnityEngine;

public class WindowController : MonoBehaviour
{
    [SerializeField] private Animator animatorLeft;
    [SerializeField] private Animator animatorRight;
    private bool isOpenleft = true;
    private bool isOpenRight = true;

    [SerializeField] private Transform player;

    [SerializeField] private BoxCollider leftWindow;
    [SerializeField] private BoxCollider rightWindow;

     private bool playerInLeftWindow;
     private bool playerInRightWindow;

    void Start()
    {
        /*animatorLeft = GetComponentInChildren<Animator>();
        animatorRight = GetComponentInChildren<Animator>();*/
    }

    void Update()
    {
        playerInLeftWindow = leftWindow.bounds.Contains(player.position);
        playerInRightWindow = rightWindow.bounds.Contains(player.position);

        if (playerInLeftWindow && Input.GetKeyDown(KeyCode.E))// Replace with your preferred key
        {
            Debug.Log("Time to close left side");
            ToggleLeftPanels();
        }
        else if (playerInRightWindow && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Time to close right side");
            ToggleRightPanels();
        }
    }
    

    public void ToggleLeftPanels()
    {
        
        if (isOpenleft)
        {
            Debug.Log("Closing left panels.");
            animatorLeft.SetTrigger("Close"); // Trigger for closing all panels
            isOpenleft = false;
        }
        else
        {
            animatorLeft.SetTrigger("Open"); // Trigger for opening all panels
            isOpenleft = true;
        }
        
        
    }

    public void ToggleRightPanels()
    {
        if (isOpenRight)
        {
            Debug.Log("Closing right panels.");
            animatorRight.SetTrigger("Close"); // Trigger for closing all panels
            isOpenRight = false;
        }
        else
        {
            animatorRight.SetTrigger("Open"); // Trigger for opening all panels
            isOpenRight = true;
        }
    }

    public bool leftWindowIsClose()
    {
        return isOpenleft;
    }

    public bool rightWindowIsClose()
    {
        return isOpenRight;
    }
}
