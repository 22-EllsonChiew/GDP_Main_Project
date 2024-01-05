using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObjects : MonoBehaviour
{
    private Vector3 offset;
    private Camera mainCamera;
    private bool isDragging = false;
    private bool isAttached = false;
    private Rigidbody rb;
    public LayerMask groundLayerMask;

    [SerializeField] private string targetSlot;


    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        DisableAllChildren();
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
        if (!isAttached )
        {
            isDragging = true;

        }
    }
    void Update()
    {
        if (isDragging)
        {   
            // Update the object's position based on the mouse movement
            Vector3 newPosition = GetMouseWorldPosition() + offset;

            // Cast a ray from the mouse position to the ground
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
            {
                // Set the object's position to the intersection point with the ground

                newPosition.y = hit.point.y + 0.3f;

                

                transform.position = newPosition;
            }

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 10f * Time.deltaTime);

        }
    }

    Vector3 GetMouseWorldPosition()
    {
        // Get the mouse position in screen space and convert it to world space
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.y; // Set the Z position based on the camera angle
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    void OnMouseUp()
    {
        isDragging = false;
        SnapToTrigger();
    }



    void SnapToTrigger()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f); // Adjust the radius as needed

        foreach (Collider col in colliders)
        {
            // Check if the collider is a trigger
            if (col.isTrigger && !isAttached)
            {
                if (col.gameObject.CompareTag(targetSlot))
                {
                    // Snap the object to the trigger's position
                    Vector3 snapPosition = col.transform.position;
                    snapPosition.y = transform.position.y; // Keep the original Y position
                    transform.position = snapPosition;
                    transform.rotation = Quaternion.identity;
                    transform.SetParent(col.transform);
                    rb.isKinematic = true;
                    isAttached = true; // You might want to set isAttached to true here or wherever is appropriate in your logic
                    EnableAllChildren();
                    break; // Stop checking for triggers after snapping to the first one
                }
            }
            else
            {
                rb.isKinematic = false;
            }
        }
    }

    void DisableAllChildren()
    {
        // Iterate through all children and disable them
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    void EnableAllChildren()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
