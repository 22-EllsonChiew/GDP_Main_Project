using UnityEngine;

public class WindowController : MonoBehaviour
{
    [SerializeField] private Animator animatorLeft;
    [SerializeField] private Animator animatorRight;
    private bool isCloseleft = false;
    private bool isCloseRight = false;

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
        
        if (isCloseleft)
        {
            Debug.Log("Closing left panels.");
            animatorLeft.SetTrigger("Open"); // Trigger for opening all panels
            isCloseleft = false;
        }
        else
        {

            
            animatorLeft.SetTrigger("Close"); // Trigger for closing all panels
            isCloseleft = true;
            
        }
        
        
    }

    public void ToggleRightPanels()
    {
        if (isCloseRight)
        {
            animatorRight.SetTrigger("Open"); // Trigger for opening all panels
            isCloseRight = false;
        }
        else
        {
            Debug.Log("Closing right panels.");
            animatorRight.SetTrigger("Close"); // Trigger for closing all panels
            isCloseRight = true;
            
        }
    }

    public bool leftWindowIsClose()
    {
        return isCloseleft;
    }

    public bool rightWindowIsClose()
    {
        return isCloseRight;
    }
}
