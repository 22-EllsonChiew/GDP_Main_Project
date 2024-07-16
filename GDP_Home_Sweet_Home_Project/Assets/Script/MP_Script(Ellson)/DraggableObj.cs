using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObj : MonoBehaviour
{
    public Camera minigameCamera; // Reference to the specific camera
    private Vector3 screenPoint;
    private Vector3 offset;
    private bool isDragging = false;
    private bool isPlaced = false;

    void OnMouseDown()
    {
        if (!isPlaced)
        {
            screenPoint = minigameCamera.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - minigameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            isDragging = true;
        }
    }

    void OnMouseDrag()
    {
        if (!isPlaced && isDragging)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = minigameCamera.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LegSlot") && !isPlaced)
        {
            // Lock the position to the target
            transform.position = other.transform.position;
            // Optionally, make the object a child of the target for better handling
            transform.SetParent(other.transform);
            // Disable the rigidbody
            GetComponent<Rigidbody>().isKinematic = true;
            // Mark the object as placed
            isPlaced = true;
        }
    }
}

