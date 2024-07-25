using UnityEngine;
using System.Collections.Generic;

public class DraggingObjects : MonoBehaviour
{
    private Vector3 offset;
    public Camera gameCamera;
    private bool isDragging = false;
    private bool isAttached = false;
    private Rigidbody rb;
    public float hoverHeight = 4f;
    public float snapRange = 16f; // Range to check for snapping

    private List<Transform> nailSlotTransforms = new List<Transform>(); // List to hold NailSlots
    private Vector3 originalPos;
    private Quaternion originalRotation;

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
            Debug.Log("Nail slot added: " + slot.name + " at position " + slot.transform.position);
        }
        Debug.Log("Number of slots = " + slots.Length);
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
        //get the mouse position in screen space and convert it to world space
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
        Transform closestSlot = GetClosestSlot();
        if (closestSlot != null && Vector3.Distance(transform.position, closestSlot.position) <= snapRange)
        {
            SnapToPosition(closestSlot);
            Debug.Log("SNAPPING to " + closestSlot.name + " at position " + closestSlot.position);
        }
    }

    Transform GetClosestSlot()
    {
        Transform closestSlot = null;
        float closestDistance = float.MaxValue;

        foreach (Transform slot in nailSlotTransforms)
        {
            float distance = Vector3.Distance(transform.position, slot.position);
            Debug.Log("Distance to " + slot.name + " = " + distance);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSlot = slot;
            }
        }

        return closestSlot;
    }

    void SnapToPosition(Transform targetTransform)
    {
        Vector3 snapPosition = targetTransform.position;
        Debug.Log("Snapping to " + targetTransform.name + " at position " + snapPosition);
        transform.position = snapPosition;
        //rotation for the mount to be mounted correctly
        transform.rotation = targetTransform.rotation * Quaternion.Euler(90, 0, 0);
        transform.SetParent(targetTransform);
        rb.isKinematic = true;
        isAttached = true;
    }

    public void ResetObject()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        isAttached = false;
        transform.position = originalPos;
        transform.rotation = originalRotation;
    }
}