using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFurniture : MonoBehaviour
{
    public GameObject player;
    public PlayerMovement playerMovement;
    public Transform dragPos;
    public float checkRadius = 2f;
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
        //SnapPosition();
    }

    void CheckForDraggableObject()
    {
        //might have to change to only check for area in front of player
        Vector3 spherePosition = player.transform.position + player.transform.forward * (checkRadius);
        spherePosition.y -= 1f;
        Debug.Log("INTO CHECKFORDRAGGABLE");
        Collider[] hitColliders = Physics.OverlapSphere(spherePosition, checkRadius);
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
<<<<<<< HEAD
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
=======
                    carriedObject = hitCollider.gameObject;
                    playerMovement.speed = 2f;
                    break;
>>>>>>> main
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
<<<<<<< HEAD
            Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.None;
            }
            playerMovement.speed = 10f;
            StartCoroutine(Dropping());
        }
    }

    IEnumerator Dropping()
    {
        //wait for a small amount of time so the object can snap before carriedObject is set to null
        yield return new WaitForSeconds(0.5f);
        carriedObject.transform.SetParent(null);
        carriedObject = null;
    }
    //void SnapPosition()
    //{
    //    Collider[] hitColliders = Physics.OverlapSphere(carriedObject.transform.position, snapRadius);
    //    foreach (var hitCollider in hitColliders)
    //    {
    //        if (hitCollider.CompareTag("SnapPosition"))
    //        {
    //            Debug.Log("can SNAP");
    //            //carriedObject.transform.position = hitCollider.transform.position;
    //            if (canSnap == true)
    //            {
    //                Debug.Log("SNAPPIN");
    //                carriedObject.transform.position = hitCollider.transform.position;
    //            }
    //        }
    //    }
    //}

    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Vector3 spherePosition = player.transform.position + player.transform.forward * (checkRadius);
            spherePosition.y -= 1f;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spherePosition, checkRadius);
=======
            //carreid object is not parented to dragPos anymore
            carriedObject.transform.SetParent(null);
            carriedObject = null;
            //player movement speed set back to original speed
            playerMovement.speed = 3f;
>>>>>>> main
        }
    }
}