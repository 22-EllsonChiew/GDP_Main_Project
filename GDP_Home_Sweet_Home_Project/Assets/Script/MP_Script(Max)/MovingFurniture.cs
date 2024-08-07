using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFurniture : MonoBehaviour
{
    public GameObject player;
    public PlayerMovement playerMovement;
    public Transform dragPos;
    public float checkRadius = 4f;
    public float snapRadius = 1f;
    public GameObject carriedObject = null;
    public float heightOffset = 0.5f;
    public float maxDistance = 3f; // Maximum distance the object can be from the player
    public float forceStrength = 1f; // Strength of the impulse force
    public GameObject snapPos;
    public bool canSnap = false;

    private void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (carriedObject != null)
            {
                DropObject();
                canSnap = true;
            }
            else
            {
                CheckForDraggableObject();
                canSnap = false;
            }
        }

        UpdateCarriedObjectPosition();
        SnapPosition();
    }

    void CheckForDraggableObject()
    {
        Debug.Log("INTO CHECKFORDRAGGABLE");
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, checkRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Object") || hitCollider.CompareTag("Drilling"))
            {
                Debug.Log("COLLIDE WITH TAG");
                carriedObject = hitCollider.gameObject;
                playerMovement.speed = 4f;
                // Lock rotation when object is picked up
                Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                }
                break;
            }
        }
    }

    void UpdateCarriedObjectPosition()
    {
        Debug.Log("INTO UPDATECARRIED");
        if (carriedObject != null)
        {
            float distance = Vector3.Distance(dragPos.position, carriedObject.transform.position);

            if (distance < maxDistance)
            {
                Vector3 direction = player.transform.forward;
                carriedObject.GetComponent<Rigidbody>().AddForce(direction * forceStrength, ForceMode.Impulse);
            }
            else
            {
                carriedObject.GetComponent<Rigidbody>().velocity = Vector3.zero; // Stop the object
            }
        }
    }

    void DropObject()
    {
        if (carriedObject != null)
        {
            Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.None;
            }
            carriedObject.transform.SetParent(null);
            carriedObject = null;
            playerMovement.speed = 10f;
        }
    }

    void SnapPosition()
    {
        Collider[] hitColliders = Physics.OverlapSphere(carriedObject.transform.position, snapRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("SnapPosition"))
            {
                Debug.Log("can SNAP");
                carriedObject.transform.position = hitCollider.transform.position;
                if (canSnap == true)
                {
                    Debug.Log("SNAPPIN");
                    //carriedObject.transform.position = hitCollider.transform.position;
                }
            }
        }
    }
}