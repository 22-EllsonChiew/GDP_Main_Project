using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class Interaction : MonoBehaviour
{

    public delegate void TaskEventHandler(bool isTaskComplete);
    public event TaskEventHandler OnTaskInteract;

    [Header("UI References")]
    [SerializeField] private ConfirmationWindow packageUI;
    [SerializeField] private GameObject toolBoxUI;
    [SerializeField] private GameObject timeSkipUI;
    [SerializeField] private TextMeshProUGUI timeSkipUIText;
    [SerializeField] private InteractionPrompt interactionUIPrompt;

    public Animator animator;

    [Header("Camera")]
    public GameObject mainCam;
    public GameObject minigameCam;

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject miniGameCamDrill;
    [SerializeField] private GameObject miniGameCamTable;

    

    public GameObject builtChair;

    private Collider currentCollider;
    private string currentNeighbourCollider;

    public static Neighbour currentNeighbour;
    public static Neighbour closestAffectedNeighbour;

    private bool ConfirmButtonClickOnce = false;

    public static bool CanInteractWithNeighbour {  get; private set; }
    private bool IsAtElevator;
    private bool IsAtBed;

    public UnityEvent<bool> isGameStarting;

    [SerializeField] private string tagName;

    public bool drillGame = false;
    public bool hammerGame = false;
    public bool tableDrilling = false;
    private float checkRadius = 0.5f;
    public GameObject player;
    private void Start()
    {
        timeSkipUI.SetActive(false);
        animator = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        CanInteractWithNeighbour = false;

        if (playerObject != null)
        {
            isGameStarting.AddListener(isTaskComplete => playerObject.GetComponent<PlayerMovement>().CheckMinigame(isTaskComplete));
        }
        else
        {
            Debug.LogError("Player GameObject not found in the scene!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            if (CanInteractWithNeighbour)
            {
                interactionUIPrompt.DisablePanel();
                ChatManager.instance.UnlockContact(currentNeighbour.neighbourName);
                NeighbourUIController.instance.StartInteraction(currentNeighbour.neighbourName, currentNeighbour.currentMood);
            }

            if (IsAtElevator || IsAtBed)
            {
                timeSkipUI.SetActive(true);
                interactionUIPrompt.DisablePanel();

                if (IsAtElevator)
                {
                    timeSkipUIText.text = "Go to work?";
                }
                
                if (IsAtBed)
                {
                    timeSkipUIText.text = "Go to bed?";
                }
            }
        }
        CheckDistance();
    }


    private void ConfirmClicked(Collider confirmedCollider)
    {
        Debug.Log("ConfirmClicked called with: " + confirmedCollider);
        //isGameStarting.Invoke(true);

        packageUI.gameObject.SetActive(false);

        if (confirmedCollider != null) 
        {
            

            minigameCam.SetActive(true);
            mainCam.SetActive(false);

            Destroy(confirmedCollider.gameObject);
            Instantiate(builtChair, new Vector3(confirmedCollider.gameObject.transform.position.x, builtChair.transform.position.y, confirmedCollider.gameObject.transform.position.z), builtChair.transform.rotation);

        }
        //call function for minigame
    }

    private void ConfirmClickedDrillGame(Collider drillConfirmedCollider)
    {
        packageUI.gameObject.SetActive(false);

        if(drillConfirmedCollider != null)
        {
            mainCamera.SetActive(false);
            miniGameCamDrill.SetActive(true);

            Destroy(drillConfirmedCollider.gameObject);

        }
    }

    private void ConfirmClickedTableGame(Collider tableConfirmedCollider)
    {
        packageUI.gameObject.SetActive(false);

        if (tableConfirmedCollider != null)
        {
            mainCamera.SetActive(false);
            miniGameCamTable.SetActive(true);

            Destroy(tableConfirmedCollider.gameObject);

        }
    }

    private void ExitClicked()
    {
        packageUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NeighbourInteractionCollider"))
        {
            CanInteractWithNeighbour = true;
            interactionUIPrompt.EnablePanel();
            interactionUIPrompt.SetInteractionText("E", "Greet");
            Debug.Log("Player @ neighbour door");
        }

        if (other.CompareTag("Environment_Window"))
        {
            interactionUIPrompt.EnablePanel();
            interactionUIPrompt.SetInteractionText("E", "Interact");
            Debug.Log("Player @ window");
        }

        if (other.CompareTag("Environment_BulletinBoard"))
        {
            interactionUIPrompt.EnablePanel();
            interactionUIPrompt.SetInteractionText("E", "View");
            Debug.Log("Player @ board");
        }

        if (other.CompareTag("Environment_Elevator"))
        {
            IsAtElevator = true;
            interactionUIPrompt.EnablePanel();
            interactionUIPrompt.SetInteractionText("E", "Take Lift");
            Debug.Log("Player @ elevator");
        }

        if (other.CompareTag("Bed"))
        {
            IsAtBed = true;
            interactionUIPrompt.EnablePanel();
            interactionUIPrompt.SetInteractionText("E", "Sleep");
            Debug.Log("Player @ bed");
        }

    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Object") && Input.GetKey(KeyCode.E)) //check if tag of the object colliding with player is "object"
    //    {
    //        if (!ConfirmButtonClickOnce)
    //        {
    //            Package packageData = other.GetComponent<Package>();

    //            packageUI.gameObject.SetActive(true);

    //            packageUI.SetFurnitureName(packageData.furnitureName);
    //            packageUI.SetFurnitureType(packageData.GetFurnitureTypeAsString());
    //            packageUI.SetAssemblyBool(packageData.isAssemblyRequired);
    //            packageUI.SetFurniturePhoto(packageData.furniturePhoto);
    //            packageUI.SetToolRequired(packageData.toolRequired);
    //            packageUI.SetManualTips(packageData.comicStrip);

    //            packageUI.confirmButton.onClick.AddListener(() => ConfirmClicked(other));
    //            packageUI.exitButton.onClick.AddListener(ExitClicked);
    //            ConfirmButtonClickOnce = true;
    //            hammerGame = true;
    //        }
    //    }

    //    if (other.CompareTag("Chest") && Input.GetKey(KeyCode.E))
    //    {
    //        Debug.Log("Opening chest");
    //        toolBoxUI.SetActive(true);
    //        animator.SetTrigger("chestOpen");
    //    }
    //    if (other.CompareTag(tagName) && Input.GetKey(KeyCode.E))
    //    {
    //        Package packageData = other.GetComponent<Package>();

    //        packageUI.gameObject.SetActive(true);

    //        packageUI.SetFurnitureName(packageData.furnitureName);
    //        packageUI.SetFurnitureType(packageData.GetFurnitureTypeAsString());
    //        packageUI.SetAssemblyBool(packageData.isAssemblyRequired);
    //        packageUI.SetFurniturePhoto(packageData.furniturePhoto);
    //        packageUI.SetToolRequired(packageData.toolRequired);
    //        packageUI.SetManualTips(packageData.comicStrip);

    //        packageUI.confirmButton.onClick.AddListener(() => ConfirmClickedDrillGame(other)); ;
    //        packageUI.exitButton.onClick.AddListener(ExitClicked);
    //        drillGame = true;
    //    }
    //    if (other.CompareTag("TableDrilling") && Input.GetKey(KeyCode.E))
    //    {
    //        Package packageData = other.GetComponent<Package>();

    //        packageUI.gameObject.SetActive(true);

    //        packageUI.SetFurnitureName(packageData.furnitureName);
    //        packageUI.SetFurnitureType(packageData.GetFurnitureTypeAsString());
    //        packageUI.SetAssemblyBool(packageData.isAssemblyRequired);
    //        packageUI.SetFurniturePhoto(packageData.furniturePhoto);
    //        packageUI.SetToolRequired(packageData.toolRequired);
    //        packageUI.SetManualTips(packageData.comicStrip);

    //        packageUI.confirmButton.onClick.AddListener(() => ConfirmClickedTableGame(other)); ;
    //        packageUI.exitButton.onClick.AddListener(ExitClicked);
    //        tableDrilling = true;
    //    }


    //    currentCollider = other;
    //}

    void CheckDistance()
    {
        Vector3 spherePosition = player.transform.position + player.transform.forward * checkRadius;
        spherePosition.y -= 1f;
        Collider[] hitColliders = Physics.OverlapSphere(spherePosition, checkRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Object") && Input.GetKey(KeyCode.E))
            {
                if (!ConfirmButtonClickOnce)
                {
                    Package packageData = hitCollider.gameObject.GetComponent<Package>();

                    packageUI.gameObject.SetActive(true);
                    interactionUIPrompt.DisablePanel();

                    packageUI.SetFurnitureDetails(packageData);

                    packageUI.confirmButton.onClick.AddListener(() => ConfirmClicked(hitCollider));
                    packageUI.exitButton.onClick.AddListener(ExitClicked);
                    ConfirmButtonClickOnce = true;
                    hammerGame = true;
                }
            }

            if (hitCollider.CompareTag("Chest") && Input.GetKey(KeyCode.E))
            {
                Debug.Log("Opening chest");
                toolBoxUI.SetActive(true);
                animator.SetTrigger("chestOpen");
            }
            if (hitCollider.CompareTag(tagName) && Input.GetKey(KeyCode.E))
            {
                Package packageData = hitCollider.gameObject.GetComponent<Package>();

                packageUI.gameObject.SetActive(true);
                interactionUIPrompt.DisablePanel();

                packageUI.SetFurnitureDetails(packageData);

                packageUI.confirmButton.onClick.AddListener(() => ConfirmClickedDrillGame(hitCollider)); 
                packageUI.exitButton.onClick.AddListener(ExitClicked);
                drillGame = true;
            }
            if (hitCollider.CompareTag("TableDrilling") && Input.GetKey(KeyCode.E))
            {
                Package packageData = hitCollider.GetComponent<Package>();

                packageUI.gameObject.SetActive(true);
                interactionUIPrompt.DisablePanel();

                packageUI.SetFurnitureDetails(packageData);

                packageUI.confirmButton.onClick.AddListener(() => ConfirmClickedTableGame(hitCollider)); ;
                packageUI.exitButton.onClick.AddListener(ExitClicked);
                tableDrilling = true;
            }


            currentCollider = hitCollider;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NeighbourInteractionCollider"))
        {
            interactionUIPrompt.DisablePanel();
            CanInteractWithNeighbour = false;
        }

        if (other.CompareTag("Environment_Window"))
        {
            interactionUIPrompt.DisablePanel();
        }

        if (other.CompareTag("Environment_BulletinBoard"))
        {
            interactionUIPrompt.DisablePanel();
        }

        if (other.CompareTag("Environment_Elevator"))
        {
            IsAtElevator = false;
            interactionUIPrompt.DisablePanel();
        }

        if (other.CompareTag("Bed"))
        {
            IsAtBed = false;
            interactionUIPrompt.DisablePanel();
        }
        
    }
}
