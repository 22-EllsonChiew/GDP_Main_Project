using UnityEngine;
using System.Collections.Generic;

public class DraggingObjects : MonoBehaviour
{
    private Vector3 offset;
    public Camera gameCamera;
    private bool isDragging = false;
    private bool isAttached = false;
    private Rigidbody rb;
    public LayerMask groundLayerMask;
    public float hoverHeight = 4f;
    public float snapRange = 0.5f; // Range to check for snapping

    private List<Transform> nailSlotTransforms = new List<Transform>(); // List to hold NailSlots
    private Vector3 originalPos;
    private Quaternion originalRotation;
    private Transform currentNailSlot = null; // Keep track of the current slot

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameCamera = GameObject.FindGameObjectWithTag("MinigameCam").GetComponent<Camera>();
        originalPos = transform.position;
        originalRotation = transform.rotation;

        // Find all NailSlots in the scene
        GameObject[] slots = GameObject.FindGameObjectsWithTag("NailSlot");
        foreach (GameObject slot in slots)
        {
            nailSlotTransforms.Add(slot.transform);
        }
        Debug.Log("number of slots = " + slots.Length);
    }

    void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
        if (!isAttached)
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

            // Keep the original Y position
            newPosition.y = -2.5f;

            // Set the object's position to the calculated new position
            transform.position = newPosition;
            transform.rotation = Quaternion.identity;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        // Get the mouse position in screen space and convert it to world space
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = gameCamera.transform.position.y;
        Vector3 worldPosition = gameCamera.ScreenToWorldPoint(mousePosition);

        return new Vector3(worldPosition.x, hoverHeight, worldPosition.z);
    }

    void OnMouseUp()
    {
        isDragging = false;
        CheckForSnap();
    }

    void CheckForSnap()
    {
        if (currentNailSlot != null)
        {
            SnapToPosition(currentNailSlot);
            Debug.Log("SNAPPIN");
        }
    }

    void SnapToPosition(Transform targetTransform)
    {
        Vector3 snapPosition = targetTransform.position;
        snapPosition.y = transform.position.y - 1.1f;
        transform.position = snapPosition;

        transform.rotation = targetTransform.rotation * Quaternion.Euler(0, 0, 180);
        transform.SetParent(targetTransform);
        rb.isKinematic = true;
        isAttached = true;
        currentNailSlot = null;
    }

    public void ResetObject()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        isAttached = false;
        transform.position = originalPos;
        transform.rotation = originalRotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NailSlot"))
        {
            currentNailSlot = other.transform; // Set the current slot
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NailSlot"))
        {
            if (other.transform == currentNailSlot)
            {
                currentNailSlot = null; // Clear the current slot
            }
        }
    }
}