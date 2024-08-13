using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        //DragText();
        CheckForDraggableObject();
        DropObject();

        UpdateCarriedObjectPosition();
        //SnapPosition();
        Debug.Log(canSnap);
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
                inRange = true;
                if (Input.GetKeyDown(KeyCode.G) && canSnap == false)
                {
                    Debug.Log("COLLIDE WITH TAG");
                    carriedObject = hitCollider.gameObject;
                    canSnap = true;
                    //Quaternion targetRotation = Quaternion.Inverse(mainCam.transform.rotation);
                    //dragText.transform.position = carriedObject.transform.position;
                    //dragText.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.0f);
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
    }

    //void DragText()
    //{
    //    //might have to change to only check for area in front of player
    //    Vector3 spherePosition = player.transform.position + player.transform.forward * (checkRadius);
    //    spherePosition.y -= 1f;
    //    Collider[] hitColliders = Physics.OverlapSphere(spherePosition, checkRadius);
    //    foreach (var hitCollider in hitColliders)
    //    {
    //        if (hitCollider.CompareTag("Object") || hitCollider.CompareTag("Drilling"))
    //        {
    //            Quaternion targetRotation = Quaternion.Inverse(mainCam.transform.rotation);
    //            dragText.transform.position = carriedObject.transform.position;
    //            dragText.transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.0f);
    //        }
    //    }
    //}

    void UpdateCarriedObjectPosition()
    {
        if (inRange)
        {
            textObject.SetActive(true);
            if (canSnap == false)
            {
                dragText.SetText("Press G to drag");
            }
            if (canSnap == true)
            {
                dragText.SetText("Press G to stop dragging");
            }
        }
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
            if (Input.GetKeyDown(KeyCode.G) && canSnap == true)
            {
                Rigidbody rb = carriedObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.constraints = RigidbodyConstraints.None;
                }
                playerMovement.speed = 10f;
                canSnap = false;
                StartCoroutine(Dropping());
            }
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

    //private void OnDrawGizmos()
    //{
    //    if (player != null)
    //    {
    //        Vector3 spherePosition = player.transform.position + player.transform.forward * (checkRadius);
    //        spherePosition.y -= 1f;
    //        Gizmos.color = Color.green;
    //        Gizmos.DrawWireSphere(spherePosition, checkRadius);
    //    }
    //}
}