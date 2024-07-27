using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Interaction : MonoBehaviour
{

    public delegate void TaskEventHandler(bool isTaskComplete);
    public event TaskEventHandler OnTaskInteract;

    [SerializeField] private ConfirmationWindow confirmationWindow;
    [SerializeField] private GameObject ChestUI;
    public Animator animator;

    public GameObject mainCam;
    public GameObject minigameCam;

    public GameObject builtChair;

    private Collider currentCollider;
    private string currentNeighbourCollider;

    public bool CanInteractWithNeighbour {  get; private set; }

    public UnityEvent<bool> isGameStarting;


    private void Start()
    {
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
        if (CanInteractWithNeighbour && Input.GetKeyDown(KeyCode.E))
        {
            if (currentNeighbourCollider == "SherrylCollider")
            {
                NeighbourUIController.instance.StartInteraction("Sherryl", "HappyGreet");
            }
            else if (currentNeighbourCollider == "HakimCollider")
            {
                NeighbourUIController.instance.StartInteraction("Hakim", "HappyGreet");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NeighbourInteractionCollider"))
        {
            CanInteractWithNeighbour = true;
            currentNeighbourCollider = other.gameObject.name;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Object") && Input.GetKey(KeyCode.E)) //check if tag of the object colliding with player is "object"
        {
            confirmationWindow.gameObject.SetActive(true);
            confirmationWindow.confirmButton.onClick.AddListener(() => ConfirmClicked(other)); ;
            confirmationWindow.exitButton.onClick.AddListener(ExitClicked);
        }

        if (other.CompareTag("Chest") && Input.GetKey(KeyCode.E))
        {
            Debug.Log("Opening chest");
            ChestUI.SetActive(true);
            animator.SetTrigger("chestOpen");
        }

        

        currentCollider = other;
    }

    private void ConfirmClicked(Collider confirmedCollider)
    {
        //isGameStarting.Invoke(true);

        confirmationWindow.gameObject.SetActive(false);

        if (confirmedCollider != null) 
        {
            

            minigameCam.SetActive(true);
            mainCam.SetActive(false);

            Destroy(confirmedCollider.gameObject);
            Instantiate(builtChair, new Vector3(confirmedCollider.gameObject.transform.position.x, builtChair.transform.position.y, confirmedCollider.gameObject.transform.position.z), builtChair.transform.rotation);

        }
        //call function for minigame
    }

    private void ExitClicked()
    {
        confirmationWindow.gameObject.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        CanInteractWithNeighbour = false;
        currentNeighbourCollider = null;
        currentCollider = null;
    }
}
