using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovingFurniture : MonoBehaviour
{
    public GameObject player;
    public PlayerMovement playerMovement;
    public Transform dragPos;
    private float checkRadius = 0.5f;
    private float snapRadius = 1f;
    public GameObject carriedObject = null;
    private float heightOffset = 0.5f;
    //max distance the object can be from the player
    private float maxDistance = 0.2f;
    //strength of the impulse force
    [SerializeField] private float forceStrength = 100f;
    public GameObject snapPos;
    public bool canSnap = false;
    public GameObject mainCam;
    public AudioSource audioSource;
    public AudioClip dragSound;
    private bool inRange = false;
    private bool dragging = false;
    public GameObject particleObject;
    public SnapCollider snapCollider;

    private Vector3 contactPoint;

    private bool timeForNextPhase = false;


    [Header("UI Refrence")]
    [SerializeField] private InteractionPrompt interactionUIPrompt;
    [SerializeField] private ConfirmationWindow packageUI;

    [Header("CupBoard GameObject")]
    public GameObject cupBoardObject;
    public GameObject cupBoardTranslucent;
    [Header("Mirror GameObject")]
    public GameObject mirrorObject;
    public GameObject mirrorTranslucnet;
    [Header("TV Set GameObject")]
    public GameObject tvSetTable;
    //private Vector3 tvPos = new Vector3(2.137f, 0.66f, -58.057f);
    public GameObject tvSetTranslucent;
    [Header("Bar Stool GameObject")]
    public GameObject barStoolObject;
    //private Vector3 barStoolPos = new Vector3(0.31f, 0.6f, -58f);
    public GameObject barStoolTranslucent;

    public GameObject barStoolTranslucent2;
    [Header("Study Table GameObject")]
    public GameObject studyTableObject;
    public GameObject studyTableTranslucent;

    [Header("Office Chair Gameobject")]
    public GameObject officeChairObject;
    public GameObject officeChairTranslucent;

    [Header("Sofa Gameobject")]
    public GameObject sofaObject;
    public GameObject sofaTranslucent;

    [Header("Lamp Gameobject")]
    public GameObject lampObject;
    public GameObject lampLRTranslucent;
    public GameObject lampBRTranslucent;

    [Header("Dining Chair")]
    public GameObject diningChairTranslucent;
    public GameObject diningChair2Translucent;
    public GameObject diningChair3Translucent;
    public GameObject diningChair4Translucent;

    [Header("Dining Table")]
    public GameObject diningTableTranslucent;

    private void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        //snapCollider.prefabPosition.y -= 2f;

        
        TranslucentObject();

    }

    private void Update()
    {
        CheckForDraggableObject();
        HandleDragging();
        SnapPosition();
    }

    void FixedUpdate()
    {
        UpdateCarriedObjectPosition();
    }

    public readonly HashSet<string> draggingTags = new HashSet<string>
    {
        "Object", "Drilling", "Draggable", "DraggableMirror", "DraggableBarStool", "DraggableTvTable", "DraggableStudyTable", "DraggableBarStool2", "DraggableOfficeChair", "DraggableSofa", "DraggableLRLamp", "DraggableBRLamp", "DraggableDiningChair",
        "DraggableDC2", "DraggableDC3", "DraggableDC4", "DraggableDiningTable"
    };

    void CheckForDraggableObject()
    {
        Vector3 spherePosition = player.transform.position + player.transform.forward * checkRadius;
        spherePosition.y -= 1f;

        Collider[] hitColliders = Physics.OverlapSphere(spherePosition, checkRadius);
        //reset check for in range of object
        inRange = false;

        foreach (var hitCollider in hitColliders)
        {
            if(draggingTags.Contains(hitCollider.tag))
            {
                inRange = true;
                //set text to active and text to press G to drags
                //exit loop early if we found a valid object
                return;
            }
        }
    }

    void HandleDragging()
    {
        if(TimeController.instance.isPaused)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (carriedObject == null && inRange)
            {
                //pick up object if there is not already a carriedObject
                PickUpObject();
                dragging = true;
                //ApplyForce();
            }
            else if (carriedObject != null)
            {
                //drop object if there is already a carriedObject
                DropObject();
                dragging = false;
            }
        }
    }

    void PickUpObject()
    {
        //sphere in front of player to check for draggable objects
        Vector3 spherePosition = player.transform.position + player.transform.forward * checkRadius;
        spherePosition.y -= 1f;

        Collider[] hitColliders = Physics.OverlapSphere(spherePosition, checkRadius);

        foreach (var hitCollider in hitColliders)
        {
            if (draggingTags.Contains(hitCollider.tag))
            {
                //carried object set to the game object that is collided
                carriedObject = hitCollider.gameObject;
                //player speed is slower when dragging
                playerMovement.speed = 2f;
                canSnap = false;

                contactPoint = hitCollider.ClosestPoint(player.transform.position);

                //play dragging sound
                if (dragSound != null && audioSource != null)
                {
                    audioSource.clip = dragSound;
                    audioSource.Play();
                }

                //lock rotation when object is picked up
                Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                }
                break;
            }
        }
    }

    void DropObject()
    {
        if (carriedObject != null)
        {
            Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
            if (rb != null)
                //remove previously set constraints on rotation of object
            {
                rb.constraints = RigidbodyConstraints.None;
            }
            //player speed set back to normal
            playerMovement.speed = 3f;
            //object can be snapped into a snap position when dropped
            canSnap = true;
            //stop dragging sound
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            StartCoroutine(Dropping());
        }
    }

    IEnumerator Dropping()
    {
        if (carriedObject != null)
        {
            yield return new WaitForSeconds(0.1f);
            carriedObject.transform.SetParent(null);
            carriedObject = null;
            particleObject.SetActive(false);
        }
            
    }

    void UpdateCarriedObjectPosition()
    {

        if (carriedObject != null)
        {
            //Calculate the direction of the push based on the player's movement input. It uses the horizontal and vertical input axes to determine the direction of the push.
            Vector3 pushDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            //Apply force in the direction of the push. To control the strength of the push, the force is multiply forceStrength, use accleration as a force instead of Impulse as it create a instantaneous impulse
            carriedObject.GetComponent<Rigidbody>().AddForce(pushDirection * forceStrength, ForceMode.Acceleration);

            //Optionally, update the contact point for continuous force application if needed
            contactPoint = carriedObject.transform.position;

            //Check the distance between the drag position and the object's position
            float distance = Vector3.Distance(dragPos.position, carriedObject.transform.position);
            //If its greater then the maxDistance, it will then reset the velocity to 0, making it the object stop moving
            if (distance > maxDistance * 2) // increase the max distance or modify the condition
            {
                carriedObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }

            TranslucentSetActive();



        }
    }

    public void TranslucentSetActive()
    {
        if(draggingTags.Contains(carriedObject.tag))
        {
            switch (carriedObject.tag)
            {
                case "Draggable":
                    particleObject.SetActive(true);
                    cupBoardTranslucent.SetActive(true);
                    particleObject.transform.position = snapCollider.prefabPosition;
                    break;
                case "DraggableMirror":
                    mirrorTranslucnet.SetActive(true);
                    break;
                case "DraggableBarStool":
                    barStoolTranslucent.SetActive(true);
                    break;
                case "DraggableTvTable":
                    tvSetTranslucent.SetActive(true);
                    break;
                case "DraggableStudyTable":
                    studyTableTranslucent.SetActive(true);
                    break;
                case "DraggableBarStool2":
                    barStoolTranslucent2.SetActive(true);
                    break;
                case "DraggableOfficeChair":
                    officeChairTranslucent.SetActive(true);
                    break;
                case "DraggableSofa":
                    sofaTranslucent.SetActive(true);
                    break;
                case "DraggableLRLamp":
                    lampLRTranslucent.SetActive(true);
                    break;
                case "DraggableBRLamp":
                    lampBRTranslucent.SetActive(true);
                    break;
                case "DraggableDiningChair":
                    diningChairTranslucent.SetActive(true);
                    break;
                case "DraggableDC2":
                    diningChair2Translucent.SetActive(true);
                    break;
                case "DraggableDC3":
                    diningChair3Translucent.SetActive(true);
                    break;
                case "DraggableDC4":
                    diningChair4Translucent.SetActive(true);
                    break;
                case "DraggableDiningTable":
                    diningTableTranslucent.SetActive(true);
                    break;

            }
        }
    }

    private void TranslucentObject()
    {
        if (cupBoardTranslucent != null)
        {
            cupBoardTranslucent.SetActive(false);
        }

        if (mirrorTranslucnet != null)
        {
            mirrorTranslucnet.SetActive(false);
        }

        if (barStoolTranslucent != null)
        {
            barStoolTranslucent.SetActive(false);
        }

        if (tvSetTranslucent != null)
        {
            tvSetTranslucent.SetActive(false);
        }

        if (studyTableTranslucent != null)
        {
            studyTableTranslucent.SetActive(false);
        }

        if (barStoolTranslucent2 != null)
        {
            barStoolTranslucent2.SetActive(false);
        }

        if (officeChairTranslucent != null)
        {
            officeChairTranslucent.SetActive(false);
        }

        if (sofaTranslucent != null)
        {
            sofaTranslucent.SetActive(false);
        }

        if (lampBRTranslucent != null)
        {
            lampBRTranslucent.SetActive(false);
        }

        if (lampLRTranslucent != null)
        {
            lampLRTranslucent.SetActive(false);
        }

        if (diningChairTranslucent != null)
        {
            diningChairTranslucent.SetActive(false);
        }

        if (diningChair2Translucent != null)
        {
            diningChair2Translucent.SetActive(false);
        }

        if (diningChair3Translucent != null)
        {
            diningChair3Translucent.SetActive(false);
        }

        if (diningChair4Translucent != null)
        {
            diningChair4Translucent.SetActive(false);
        }

        if (diningTableTranslucent != null)
        {
            diningTableTranslucent.SetActive(false);
        }
        
    }
    
    void SnapPosition()
    {
        if (carriedObject == null) return;
        //check for snap colliders in a radius
        Collider[] hitColliders = Physics.OverlapSphere(carriedObject.transform.position, snapRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("SnapPosition"))
            {
                //if canSnap is true
                if (canSnap)
                {
                    //carriedObject will be set to the snap collider's position
                    carriedObject.transform.position = hitCollider.transform.position;
                }
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        Package packageData = other.gameObject.GetComponent<Package>();
        GameObject targetObject = other.gameObject;

        if (Input.GetKeyDown(KeyCode.E) && packageData != null && targetObject != null)
        {
            packageUI.gameObject.SetActive(true);
            packageUI.SetFurnitureDetails(packageData);

            packageUI.confirmButton.onClick.RemoveAllListeners();
            packageUI.confirmButton.onClick.AddListener(() => InstantiateFurniture(packageData, targetObject));
            packageUI.confirmButton.onClick.AddListener(() => ClosePackageManualUI());
        }
        
    }

    void ClosePackageManualUI()
    {
        if (packageUI.gameObject.activeSelf)
        {
            packageUI.gameObject.SetActive(false);
        }
    }

    void InstantiateFurniture(Package packageData, GameObject other)
    {
        if (packageData != null)
        {
            if (packageData.furnitureType == FurnitureType.Large_Cabinet)
            {
                Vector3 cupBoardPos = other.transform.position;
                cupBoardPos.y = 0.7811141f;
                other.SetActive(false);

                GameObject instantiatedObject = Instantiate(cupBoardObject, cupBoardPos, Quaternion.identity);
                ScoreManager.Instance.IncrementTotalFunitureCount();

            }

            if (packageData.furnitureType == FurnitureType.Mirror)
            {
                Vector3 mirrorPos = other.transform.position;
                mirrorPos.y = 0.8f;
                other.SetActive(false);

                GameObject instantiatedMirrorObject = Instantiate(mirrorObject, mirrorPos, Quaternion.identity);
                ScoreManager.Instance.IncrementTotalFunitureCount();
            }

            if (packageData.furnitureType == FurnitureType.Bar_Stool)
            {
                Vector3 barStoolPos = other.transform.position;
                other.SetActive(false);

                GameObject instantiatedBarStool = Instantiate(barStoolObject, barStoolPos, Quaternion.identity);
                ScoreManager.Instance.IncrementTotalFunitureCount();

            }

            if (packageData.furnitureType == FurnitureType.TV_Console)
            {
                Vector3 tvPos = other.transform.position;
                other.SetActive(false);

                GameObject instantiatedTvSet = Instantiate(tvSetTable, tvPos, Quaternion.identity);
                ScoreManager.Instance.IncrementTotalFunitureCount();
            }
            if (packageData.furnitureType == FurnitureType.Study_Table)
            {
                Vector3 studyTable = other.transform.position;
                other.SetActive(false);

                GameObject instantiatedStudyTable = Instantiate(studyTableObject, studyTable, Quaternion.identity);
                ScoreManager.Instance.IncrementTotalFunitureCount();
            }

            if (packageData.furnitureType == FurnitureType.Office_Chair)
            {
                Vector3 officeChairPos = other.transform.position;
                other.SetActive(false);

                GameObject instantiatedOfficeChair = Instantiate(officeChairObject, officeChairPos, Quaternion.identity);
                ScoreManager.Instance.IncrementTotalFunitureCount();
            }
            if (packageData.furnitureType == FurnitureType.Sofa)
            {
                Vector3 sofaPos = other.transform.position;
                other.SetActive(false);

                GameObject instantiatedSofa = Instantiate(sofaObject, sofaPos, Quaternion.identity);
                ScoreManager.Instance.IncrementTotalFunitureCount();
            }
            if (packageData.furnitureType == FurnitureType.Lamp)
            {
                Vector3 lampPos = other.transform.position;
                other.SetActive(false);
                GameObject instantiatedLamp = Instantiate(lampObject, lampPos, Quaternion.identity);
                ScoreManager.Instance.IncrementTotalFunitureCount();
            }
        }
        else
        {
            Debug.LogWarning("Package data not found!");
        }
    }


    private void EnableInteractionUI()
    {
        interactionUIPrompt.EnablePanel();
        interactionUIPrompt.SetInteractionText("E/G", "Interact/Drag");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(draggingTags.Contains(other.tag))
        {
            EnableInteractionUI();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(draggingTags.Contains(other.tag))
        {
            EnableInteractionUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (draggingTags.Contains(other.tag))
        {
            interactionUIPrompt.DisablePanel();
        }
    }
}