using UnityEngine;

public class WindowController : MonoBehaviour
{
    [SerializeField] private Animator animatorLeft;
    [SerializeField] private Animator animatorRight;
    [SerializeField] private Animator animatorKitchen;
    [SerializeField] private Animator animatorBedRoom;

    private bool isCloseKitchen = false;
    private bool isCloseBedRoom = false;
    private bool isCloseleft = false;
    private bool isCloseRight = false;

    [SerializeField] private Transform player;

    [SerializeField] private BoxCollider leftWindow;
    [SerializeField] private BoxCollider rightWindow;
    [SerializeField] private BoxCollider kitchenWindow;
    [SerializeField] private BoxCollider bedRoomWindow;

    private bool playerInLeftWindow;
    private bool playerInRightWindow;
    private bool playerInKitchenWindow;
    private bool playerInBedRoomWindow;

    private KeyCode interactWindow = KeyCode.E;


    void Start()
    {
        /*animatorLeft = GetComponentInChildren<Animator>();
        animatorRight = GetComponentInChildren<Animator>();*/
    }

    void Update()
    {
        BoxColliderWindows();

        if (playerInLeftWindow && Input.GetKeyDown(interactWindow))// Replace with your preferred key
        {
            Debug.Log("Time to close left side");
            ToggleLeftPanels();
        }
        else if (playerInRightWindow && Input.GetKeyDown(interactWindow))
        {
            Debug.Log("Time to close right side");
            ToggleRightPanels();
        }
        else if(playerInKitchenWindow && Input.GetKeyDown(interactWindow))
        {
            Debug.Log("Time to close Kitchen side");
            ToggleKitchenWindow();
        }
        else if(playerInBedRoomWindow && Input.GetKeyDown(interactWindow))
        {
            Debug.Log("Time to close BedRoom side");
            ToggleBedRoomWindow();
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

    public void ToggleKitchenWindow()
    {
        if (isCloseKitchen)
        {
            animatorRight.SetTrigger("Open"); // Trigger for opening all panels
            isCloseKitchen = false;
        }
        else
        {
            Debug.Log("Closing right panels.");
            animatorRight.SetTrigger("Close"); // Trigger for closing all panels
            isCloseKitchen = true;

        }
    }

    public void ToggleBedRoomWindow()
    {
        if (isCloseBedRoom)
        {
            animatorRight.SetTrigger("Open"); // Trigger for opening all panels
            isCloseBedRoom = false;
        }
        else
        {
            Debug.Log("Closing right panels.");
            animatorRight.SetTrigger("Close"); // Trigger for closing all panels
            isCloseBedRoom = true;

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

    public bool kitchenWindowIsClose()
    {
        return isCloseKitchen;
    }

    public bool bedRoomWindowIsClose()
    {
        return isCloseBedRoom;
    }

    private void BoxColliderWindows()
    {
        playerInLeftWindow = leftWindow.bounds.Contains(player.position);
        playerInRightWindow = rightWindow.bounds.Contains(player.position);
        playerInKitchenWindow = kitchenWindow.bounds.Contains(player.position);
        playerInBedRoomWindow = bedRoomWindow.bounds.Contains(player.position);

    }
}
