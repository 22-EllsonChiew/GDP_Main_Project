using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MovingFurniture : MonoBehaviour
{
    public GameObject player;
    public PlayerMovement playerMovement;
    public Transform dragPos;
    public float checkRadius = 0.05f;
    public float snapRadius = 1f;
    public GameObject carriedObject = null;
    public float heightOffset = 0.5f;
    //max distance the object can be from the player
    public float maxDistance = 3f;
    //strength of the impulse force
    public float forceStrength = 1f;
    public GameObject snapPos;
    public bool canSnap = false;
    public GameObject textObject;
    public TextMeshPro dragText;
    public GameObject mainCam;
    private bool inRange = false;

    private void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        CheckForDraggableObject(); 
        HandleDragging();         
        UpdateCarriedObjectPosition();
        SnapPosition();
    }

    void CheckForDraggableObject()
    {
        Vector3 spherePosition = player.transform.position + player.transform.forward * checkRadius;
        spherePosition.y -= 1f;

        Collider[] hitColliders = Physics.OverlapSphere(spherePosition, checkRadius);
        //reset check for in range of object
        inRange = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Object") || hitCollider.CompareTag("Drilling") || hitCollider.CompareTag("Draggable") || hitCollider.CompareTag("DraggableMirror") || hitCollider.CompareTag("DraggableBarStool") || hitCollider.CompareTag("DraggableTvTable"))
            {
                inRange = true; 
                //set text to active and text to press G to drag
                textObject.SetActive(true); 
                dragText.SetText("Press G to drag");
                //exit loop early if we found a valid object
                return; 
            }
        }

        // Hide the text if not in range of any object
        if (!inRange && carriedObject == null)
        {
            textObject.SetActive(false);
        }
    }

    void HandleDragging()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (carriedObject == null && inRange)
            {
                //pick up object if there is not already a carriedObject
                PickUpObject();
            }
            else if (carriedObject != null)
            {
                //drop object if there is already a carriedObject
                DropObject();
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
            if (hitCollider.CompareTag("Object") || hitCollider.CompareTag("Drilling") || hitCollider.CompareTag("Draggable") || hitCollider.CompareTag("DraggableMirror") || hitCollider.CompareTag("DraggableBarStool") || hitCollider.CompareTag("DraggableTvTable"))
            {
                //carried object set to the game object that is collided
                carriedObject = hitCollider.gameObject;
                //player speed is slower when dragging
                playerMovement.speed = 2f;
                canSnap = false;

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
            StartCoroutine(Dropping());
        }
    }

    IEnumerator Dropping()
    {
        yield return new WaitForSeconds(0.5f);
        carriedObject.transform.SetParent(null);
        carriedObject = null;
    }

    void UpdateCarriedObjectPosition()
    {
        if (carriedObject != null)
        {
            float distance = Vector3.Distance(dragPos.position, carriedObject.transform.position);

            if (distance < maxDistance)
            {
                //apply an impulse force on the object in the players forward direction
                Vector3 direction = player.transform.forward;
                carriedObject.GetComponent<Rigidbody>().AddForce(direction * forceStrength, ForceMode.Impulse);
            }
            else
            {
                //stop the object
                carriedObject.GetComponent<Rigidbody>().velocity = Vector3.zero; 
            }

            //update text while carrying
            dragText.SetText("Press G to drop");
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
}