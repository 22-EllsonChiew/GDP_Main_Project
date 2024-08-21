using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Interaction : MonoBehaviour
{

    public delegate void TaskEventHandler(bool isTaskComplete);
    public event TaskEventHandler OnTaskInteract;

    [SerializeField] private ConfirmationWindow packageUI;
    [SerializeField] private GameObject toolBoxUI;
    [SerializeField] private GameObject timeSkipUI;

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
                NeighbourUIController.instance.StartInteraction(currentNeighbour.neighbourName, currentNeighbour.currentMood);
            }

            if (IsAtElevator || IsAtBed)
            {
                timeSkipUI.SetActive(true);
            }
        }

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
            Debug.Log("Player @ neighbour door");
        }

        if (other.CompareTag("Neighbour"))
        { 

        }

        if (other.CompareTag("Environment_Elevator"))
        {
            IsAtElevator = true;
            Debug.Log("Player @ elevator");
        }

        if (other.CompareTag("Bed"))
        {
            IsAtBed = true;
            Debug.Log("Player @ bed");
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Object") && Input.GetKey(KeyCode.E)) //check if tag of the object colliding with player is "object"
        {
            if (!ConfirmButtonClickOnce)
            {
                packageUI.gameObject.SetActive(true);
                packageUI.confirmButton.onClick.AddListener(() => ConfirmClicked(other));
                packageUI.exitButton.onClick.AddListener(ExitClicked);
                ConfirmButtonClickOnce = true;
                hammerGame = true;
            }
        }

        if (other.CompareTag("Chest") && Input.GetKey(KeyCode.E))
        {
            Debug.Log("Opening chest");
            toolBoxUI.SetActive(true);
            animator.SetTrigger("chestOpen");
        }
        if (other.CompareTag(tagName) && Input.GetKey(KeyCode.E))
        {
            Package packageData = other.GetComponent<Package>();

            packageUI.gameObject.SetActive(true);

            packageUI.SetFurnitureName(packageData.furnitureName);
            packageUI.SetFurnitureType(packageData.furnitureType);
            packageUI.SetAssemblyBool(packageData.isAssemblyRequired);
            packageUI.SetFurniturePhoto(packageData.furniturePhoto);
            packageUI.SetToolRequired(packageData.toolRequired);
            packageUI.SetManualTips(packageData.comicStrip);

            packageUI.confirmButton.onClick.AddListener(() => ConfirmClickedDrillGame(other)); ;
            packageUI.exitButton.onClick.AddListener(ExitClicked);
            drillGame = true;
        }
        if (other.CompareTag("TableDrilling") && Input.GetKey(KeyCode.E))
        {
            Package packageData = other.GetComponent<Package>();

            packageUI.gameObject.SetActive(true);

            packageUI.SetFurnitureName(packageData.furnitureName);
            packageUI.SetFurnitureType(packageData.furnitureType);
            packageUI.SetAssemblyBool(packageData.isAssemblyRequired);
            packageUI.SetFurniturePhoto(packageData.furniturePhoto);
            packageUI.SetToolRequired(packageData.toolRequired);
            packageUI.SetManualTips(packageData.comicStrip);

            packageUI.confirmButton.onClick.AddListener(() => ConfirmClickedTableGame(other)); ;
            packageUI.exitButton.onClick.AddListener(ExitClicked);
            tableDrilling = true;
        }


        currentCollider = other;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NeighbourInteractionCollider"))
        {
            CanInteractWithNeighbour = false;
        }

        if (other.CompareTag("Environment_Elevator"))
        {
            
        }

        if (other.CompareTag("Bed"))
        {
            
        }
        
    }
}
