using UnityEngine;

public class WindowController : MonoBehaviour
{

    [Header("Window Animator References")]
    [SerializeField] private Animator animatorLeft;
    [SerializeField] private Animator animatorRight;
    [SerializeField] private Animator animatorKitchen;
    [SerializeField] private Animator animatorBedRoom;

    private bool isCloseKitchen = false;
    private bool isCloseBedRoom = false;
    private bool isCloseleft = false;
    private bool isCloseRight = false;

    [SerializeField] private Transform player;

    [Header("Window Collider References")]
    [SerializeField] private BoxCollider leftWindow;
    [SerializeField] private BoxCollider rightWindow;
    [SerializeField] private BoxCollider kitchenWindow;
    [SerializeField] private BoxCollider bedRoomWindow;

    private bool playerInLeftWindow;
    private bool playerInRightWindow;
    private bool playerInKitchenWindow;
    private bool playerInBedRoomWindow;

    public int windowClosedCount = 0;

    private KeyCode interactWindow = KeyCode.E;


    void Start()
    {
        /*animatorLeft = GetComponentInChildren<Animator>();
        animatorRight = GetComponentInChildren<Animator>();*/

        //ToggleKitchenWindow();
        //ToggleBedRoomWindow();
    }

    void Update()
    {
        BoxColliderWindows();

        if (playerInLeftWindow && Input.GetKeyDown(interactWindow))// Replace with your preferred key
        {
            Debug.Log("Time to close left side");
            ToggleLeftPanels();
            windowClosedCount++;
        }
        else if (playerInRightWindow && Input.GetKeyDown(interactWindow))
        {
            Debug.Log("Time to close right side");
            ToggleRightPanels();
            windowClosedCount++;
        }
        else if(playerInKitchenWindow && Input.GetKeyDown(interactWindow))
        {
            Debug.Log("Time to close Kitchen side");
            ToggleKitchenWindow();
            windowClosedCount++;
        }
        else if(playerInBedRoomWindow && Input.GetKeyDown(interactWindow))
        {
            Debug.Log("Time to close BedRoom side");
            ToggleBedRoomWindow();
            windowClosedCount++;
        }

    }

    public float NoiseLevel()
    {
        switch(windowClosedCount)
        {
            case 0:
                return 0.55f;
            case 1:
                return 0.50f;
            case 2:
                return 0.45f;
            case 3:
                return 0.40f;
            case 4:
                return 0.35f;
            default:
                return 0.60f;
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
            animatorKitchen.SetTrigger("Open"); // Trigger for opening all panels
            isCloseKitchen = false;
        }
        else
        {
            Debug.Log("Closing Kitchen panels.");
            animatorKitchen.SetTrigger("Close"); // Trigger for closing all panels
            isCloseKitchen = true;

        }
    }

    public void ToggleBedRoomWindow()
    {
        if (isCloseBedRoom)
        {
            animatorBedRoom.SetTrigger("Open"); // Trigger for opening all panels
            isCloseBedRoom = false;
        }
        else
        {
            Debug.Log("Closing right panels.");
            animatorBedRoom.SetTrigger("Close"); // Trigger for closing all panels
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
