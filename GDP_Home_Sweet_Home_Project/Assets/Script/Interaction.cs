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

    [Header("Sound Effects")]
    public AudioClip sfx_ToolboxOpen;
    public AudioClip sfx_ToolboxClose;
    public AudioClip sfx_PackageManualOpen;

    public Animator animator;

    [Header("Camera")]
    public GameObject mainCam;
    public GameObject minigameCam;

    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject miniGameCamDrill;
    [SerializeField] private GameObject miniGameCamTable;
    [SerializeField] private GameObject miniGameCam2;
    [SerializeField] private GameObject miniGameCam3;
    [SerializeField] private GameObject miniGameCam4;


    [Header("Buildable prefab")]
    public GameObject builtChair;
    public GameObject builtChair2;
    public GameObject builtChair3;
    public GameObject builtChair4;
    public GameObject builtShelf;
    public GameObject builtTable;

    [Header("Shelf Position and Rotation")]
    public Vector3 shelfPos = new Vector3(0, 0, 0);
    public Vector3 ShelfRot = new Vector3(0, 0, 0);

    private bool builtChair3Instantiated = false;

    private Collider currentCollider;
    private string currentNeighbourCollider;

    public static Neighbour currentNeighbour;
    public static Neighbour closestAffectedNeighbour;

    private bool ConfirmButtonClickOnce = false;

    public static bool CanInteractWithNeighbour {  get; private set; }
    private bool IsAtElevator;
    private bool IsAtBed;
    private bool IsAtToolbox;

    public UnityEvent<bool> isGameStarting;

    [SerializeField] private string tagName;

    public bool drillGame = false;
    public bool hammerGame = false;
    public bool hammerGame2 = false;
    public bool hammerGame3 = false;
    public bool hammerGame4 = false;

    public bool tableDrilling = false;
    private float checkRadius = 0.5f;
    public GameObject player;
    private bool inPackageUI = false;
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
                NeighbourUIController.instance.StartInteraction(currentNeighbour.neighbourName, currentNeighbour.CurrentMood);
            }

            if (IsAtElevator && TimeController.instance.CurrentTimePhase == TimePhase.Morning)
            {
                timeSkipUI.SetActive(true);
                interactionUIPrompt.DisablePanel();
                timeSkipUIText.text = "Go to work?";
            }

            if (IsAtBed && TimeController.instance.CurrentTimePhase != TimePhase.Morning)
            {
                timeSkipUI.SetActive(true);
                interactionUIPrompt.DisablePanel();
                timeSkipUIText.text = "Go to bed?";
            }

            if (IsAtToolbox)
            {
                Debug.Log("Opening chest");
                AudioManager.Instance.PlaySFX(sfx_ToolboxOpen);
                toolBoxUI.SetActive(true);
                animator.SetTrigger("chestOpen");
            }
        }
        CheckDistance();
    }


    private void ConfirmClicked(Collider confirmedCollider)
    {
       
        //isGameStarting.Invoke(true);

        packageUI.gameObject.SetActive(false);
        interactionUIPrompt.DisablePanel();

        if (confirmedCollider != null) 
        {
            Debug.Log("Hello there");

            ConfirmButtonClickOnce = true;
            minigameCam.SetActive(true);
            mainCam.SetActive(false);

            Destroy(confirmedCollider.gameObject);

            if(TimeController.CurrentDay == 1)
            {
                Instantiate(builtChair, new Vector3(confirmedCollider.gameObject.transform.position.x, builtChair.transform.position.y, confirmedCollider.gameObject.transform.position.z), builtChair.transform.rotation);
            }
            

            if(TimeController.CurrentDay == 2)
            {
                Instantiate(builtChair2, new Vector3(confirmedCollider.gameObject.transform.position.x, builtChair2.transform.position.y, confirmedCollider.gameObject.transform.position.z), builtChair2.transform.rotation);
            }


            if(TimeController.CurrentDay == 3)
            {
                if (!builtChair3Instantiated)
                {
                    Instantiate(builtChair3, new Vector3(confirmedCollider.gameObject.transform.position.x, builtChair3.transform.position.y, confirmedCollider.gameObject.transform.position.z), builtChair3.transform.rotation);
                    builtChair3Instantiated = true; // Set the flag to true after instantiation
                }
                else
                {
                    Instantiate(builtChair4, new Vector3(confirmedCollider.gameObject.transform.position.x, builtChair4.transform.position.y, confirmedCollider.gameObject.transform.position.z), builtChair4.transform.rotation);
                }
            }

        }
        //call function for minigame
    }

    private void ConfirmClicked2(Collider comfirmCollider2)
    {
        packageUI.gameObject.SetActive(false);
        interactionUIPrompt.DisablePanel();

        if (comfirmCollider2 != null)
        {
            Debug.Log("Hello there");

            ConfirmButtonClickOnce = true;
            minigameCam.SetActive(true);
            mainCam.SetActive(false);

            Destroy(comfirmCollider2.gameObject);

            


            if (TimeController.CurrentDay == 2)
            {
                Instantiate(builtChair2, new Vector3(comfirmCollider2.gameObject.transform.position.x, builtChair2.transform.position.y, comfirmCollider2.gameObject.transform.position.z), builtChair2.transform.rotation);
            }



        }

    }

    private void ConfirmClickedDrillGame(Collider drillConfirmedCollider)
    {
        packageUI.gameObject.SetActive(false);
        interactionUIPrompt.DisablePanel();

        if (drillConfirmedCollider != null)
        {
            mainCamera.SetActive(false);
            miniGameCamDrill.SetActive(true);

            Destroy(drillConfirmedCollider.gameObject);
            Debug.Log("SPAWN BABY");
            Quaternion shelfPrefab = Quaternion.Euler(ShelfRot);
            GameObject shelfInstantiate = Instantiate(builtShelf, shelfPos, shelfPrefab);

        }
    }

    private void ConfirmClickedTableGame(Collider tableConfirmedCollider)
    {
        packageUI.gameObject.SetActive(false);
        interactionUIPrompt.DisablePanel();

        if (tableConfirmedCollider != null)
        {
            mainCamera.SetActive(false);
            miniGameCamTable.SetActive(true);

            Destroy(tableConfirmedCollider.gameObject);
            Instantiate(builtTable, new Vector3(tableConfirmedCollider.gameObject.transform.position.x, builtChair4.transform.position.y, tableConfirmedCollider.gameObject.transform.position.z), builtChair4.transform.rotation);
        }
    }

    private void ExitClicked()
    {
        Debug.Log("LEAVING PACKAGE UI");
        packageUI.gameObject.SetActive(false);
        inPackageUI = false;
        ConfirmButtonClickOnce = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            interactionUIPrompt.EnablePanel();
            interactionUIPrompt.SetInteractionText("E", "Open");
        }

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

        if (other.CompareTag("Environment_Elevator") && TimeController.instance.CurrentTimePhase == TimePhase.Morning)
        {
            IsAtElevator = true;
            interactionUIPrompt.EnablePanel();
            interactionUIPrompt.SetInteractionText("E", "Take Lift");
            Debug.Log("Player @ elevator");
        }

        if (other.CompareTag("Bed") && TimeController.instance.CurrentTimePhase != TimePhase.Morning)
        {
            IsAtBed = true;
            interactionUIPrompt.EnablePanel();
            interactionUIPrompt.SetInteractionText("E", "Sleep");
            Debug.Log("Player @ bed");
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            IsAtToolbox = true;
            interactionUIPrompt.EnablePanel();
            interactionUIPrompt.SetInteractionText("E", "Open");
        }

        if (other.CompareTag("NeighbourInteractionCollider"))
        {
            interactionUIPrompt.EnablePanel();
            interactionUIPrompt.SetInteractionText("E", "Greet");
        }

        if (other.CompareTag("Environment_Window"))
        {
            interactionUIPrompt.EnablePanel();
            interactionUIPrompt.SetInteractionText("E", "Interact");
        }

        if (other.CompareTag("Environment_BulletinBoard"))
        {
            interactionUIPrompt.EnablePanel();
            interactionUIPrompt.SetInteractionText("E", "View");
        }

        if (other.CompareTag("Environment_Elevator") && TimeController.instance.CurrentTimePhase == TimePhase.Morning)
        {
            interactionUIPrompt.EnablePanel();
            interactionUIPrompt.SetInteractionText("E", "Take Lift");
        }

        if (other.CompareTag("Bed") && TimeController.instance.CurrentTimePhase != TimePhase.Morning)
        {
            interactionUIPrompt.EnablePanel();
            interactionUIPrompt.SetInteractionText("E", "Sleep");
        }
    }

    public void CloseToolboxUI() 
    {
        if (toolBoxUI.activeSelf)
        {
            AudioManager.Instance.PlaySFX(sfx_ToolboxClose);
            toolBoxUI.SetActive(false);
        }
    }

    public void ClosePackageManualUI()
    {
        if (packageUI.gameObject.activeSelf)
        {
            packageUI.gameObject.SetActive(false);
        }
    }

    public void CloseTimeSkipUI()
    {
        if (timeSkipUI.activeSelf)
        {
            timeSkipUI.SetActive(false);
        }
    }

    void CheckDistance()
    {

        Vector3 spherePosition = player.transform.position + player.transform.forward * checkRadius;
        spherePosition.y -= 1f;
        Collider[] hitColliders = Physics.OverlapSphere(spherePosition, checkRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Object") && Input.GetKeyDown(KeyCode.E))
            {
                if (!ConfirmButtonClickOnce && inPackageUI == false)
                {
                    Debug.Log("IN PACKAGE UI");
                    inPackageUI = true;
                    Package packageData = hitCollider.gameObject.GetComponent<Package>();
                    AudioManager.Instance.PlaySFX(sfx_PackageManualOpen);
                    packageUI.gameObject.SetActive(true);
                    interactionUIPrompt.DisablePanel();

                    packageUI.SetFurnitureDetails(packageData);

                    packageUI.confirmButton.onClick.AddListener(() => ConfirmClicked(hitCollider));
                    packageUI.exitButton.onClick.AddListener(ExitClicked);
                    hammerGame = true;
                }
            }

            if (hitCollider.CompareTag(tagName) && Input.GetKeyDown(KeyCode.E))
            {
                Package packageData = hitCollider.gameObject.GetComponent<Package>();
                AudioManager.Instance.PlaySFX(sfx_PackageManualOpen);
                packageUI.gameObject.SetActive(true);
                interactionUIPrompt.DisablePanel();

                packageUI.SetFurnitureDetails(packageData);

                packageUI.confirmButton.onClick.AddListener(() => ConfirmClickedDrillGame(hitCollider)); 
                packageUI.exitButton.onClick.AddListener(ExitClicked);
                drillGame = true;
            }

            if (hitCollider.CompareTag("DraggableDiningTable") && Input.GetKeyDown(KeyCode.E))
            {
                Package packageData = hitCollider.GetComponent<Package>();

                AudioManager.Instance.PlaySFX(sfx_PackageManualOpen);

                packageUI.gameObject.SetActive(true);
                interactionUIPrompt.DisablePanel();

                packageUI.SetFurnitureDetails(packageData);

                packageUI.confirmButton.onClick.AddListener(() => ConfirmClickedTableGame(hitCollider)); ;
                packageUI.exitButton.onClick.AddListener(ExitClicked);
                tableDrilling = true;
            }
            if (hitCollider.CompareTag("Object2") && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("OBJECT 2");
                inPackageUI = true;
                Package packageData = hitCollider.gameObject.GetComponent<Package>();
                AudioManager.Instance.PlaySFX(sfx_PackageManualOpen);
                packageUI.gameObject.SetActive(true);
                interactionUIPrompt.DisablePanel();

                packageUI.SetFurnitureDetails(packageData);

                packageUI.confirmButton.onClick.AddListener(() => ConfirmClicked2(hitCollider));
                packageUI.exitButton.onClick.AddListener(ExitClicked);
                hammerGame2 = true;
            }
            if (hitCollider.CompareTag("Object3") && Input.GetKeyDown(KeyCode.E))
            {
                if (!ConfirmButtonClickOnce && inPackageUI == false)
                {
                    Debug.Log("IN PACKAGE UI");
                    inPackageUI = true;
                    Package packageData = hitCollider.gameObject.GetComponent<Package>();
                    AudioManager.Instance.PlaySFX(sfx_PackageManualOpen);
                    packageUI.gameObject.SetActive(true);
                    interactionUIPrompt.DisablePanel();

                    packageUI.SetFurnitureDetails(packageData);

                    packageUI.confirmButton.onClick.AddListener(() => ConfirmClicked(hitCollider));
                    packageUI.exitButton.onClick.AddListener(ExitClicked);
                    hammerGame3 = true;
                }
            }
            if (hitCollider.CompareTag("Object4") && Input.GetKeyDown(KeyCode.E))
            {
                if (!ConfirmButtonClickOnce && inPackageUI == false)
                {
                    Debug.Log("IN PACKAGE UI");
                    inPackageUI = true;
                    Package packageData = hitCollider.gameObject.GetComponent<Package>();
                    AudioManager.Instance.PlaySFX(sfx_PackageManualOpen);
                    packageUI.gameObject.SetActive(true);
                    interactionUIPrompt.DisablePanel();

                    packageUI.SetFurnitureDetails(packageData);

                    packageUI.confirmButton.onClick.AddListener(() => ConfirmClicked(hitCollider));
                    packageUI.exitButton.onClick.AddListener(ExitClicked);
                    hammerGame4 = true;
                }
            }


            currentCollider = hitCollider;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            IsAtToolbox = false;
            interactionUIPrompt.DisablePanel();
        }

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
