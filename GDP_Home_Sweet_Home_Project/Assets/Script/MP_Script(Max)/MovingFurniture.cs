using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFurniture : MonoBehaviour
{
    public GameObject player;
    public PlayerMovement playerMovement;
    public Transform dragPos;
    public float checkRadius = 2f;
    public GameObject carriedObject = null;
    public float heightOffset = 0.5f;
    public float maxDistance = 3f; // Maximum distance the object can be from the player

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
            }
            else
            {
                CheckForDraggableObject();
            }
        }

        UpdateCarriedObjectPosition();
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
                break;
            }
        }
    }

    void UpdateCarriedObjectPosition()
    {
        Debug.Log("INTO UPDATECARRIED");
        if (carriedObject != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(dragPos.position, Vector3.down, out hit))
            {
                Vector3 direction = (dragPos.position - carriedObject.transform.position).normalized;
                float distance = Vector3.Distance(dragPos.position, carriedObject.transform.position);

                if (distance < maxDistance)
                {
                    carriedObject.GetComponent<Rigidbody>().AddForce(direction * 4, ForceMode.Impulse);
                }
                else
                {
                    carriedObject.GetComponent<Rigidbody>().velocity = Vector3.zero; // Stop the object
                }
            }
        }
    }

    void DropObject()
    {
        if (carriedObject != null)
        {
            carriedObject.transform.SetParent(null);
            carriedObject = null;
            playerMovement.speed = 10f;
        }
    }
}