using UnityEngine;

public class WindowController : MonoBehaviour
{
    [SerializeField] private Animator animatorLeft;
    [SerializeField] private Animator animatorRight;
    private bool isOpenleft = false;
    private bool isOpenRight = false;

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
            Debug.Log("Time to close");
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
            animatorLeft.SetTrigger("CloseAllPanels"); // Trigger for closing all panels
            isOpenleft = false;
        }
        else
        {
            animatorLeft.SetTrigger("OpenAllPanels"); // Trigger for opening all panels
            isOpenleft = true;
        }
    }

    public void ToggleRightPanels()
    {
        if (isOpenRight)
        {
            Debug.Log("Closing right panels.");
            animatorRight.SetTrigger("CloseAllPanels"); // Trigger for closing all panels
            isOpenRight = false;
        }
        else
        {
            animatorRight.SetTrigger("OpenAllPanels"); // Trigger for opening all panels
            isOpenRight = true;
        }
    }
}
