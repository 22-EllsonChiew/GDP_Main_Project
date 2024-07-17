using UnityEngine;

public class DraggingObjects : MonoBehaviour
{
    private Vector3 offset;
    public Camera gameCamera;
    private bool isDragging = false;
    private bool isAttached = false;
    private Rigidbody rb;
    public LayerMask groundLayerMask;
    public float hoverHeight = 4f;
    public float snapRange = 5f; // Range to check for snapping

    private Transform nailSlotTransform;

    private Vector3 originalPos;
    private Quaternion originalRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameCamera = GameObject.FindGameObjectWithTag("MinigameCam").GetComponent<Camera>();
        originalPos = transform.position;
        originalRotation = transform.rotation;
        nailSlotTransform = GameObject.FindGameObjectWithTag("NailSlot").transform;
        if (nailSlotTransform != null)
        {
            Debug.Log("Slots present");
        }
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
            newPosition.y = transform.position.y;

            // Set the object's position to the calculated new position
            transform.position = newPosition;
            transform.rotation = Quaternion.identity;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        // Get the mouse position in screen space and convert it to world space
        Vector3 mousePosition = Input.mousePosition;
        // Set the Z position based on the camera angle
        mousePosition.z = gameCamera.transform.position.y;
        Vector3 worldPosition = gameCamera.ScreenToWorldPoint(mousePosition);

        // Return the X and Z position, and maintain the object's hover height
        return new Vector3(worldPosition.x, hoverHeight, worldPosition.z);
    }

    void OnMouseUp()
    {
        isDragging = false;
        CheckForSnap();
    }

    void CheckForSnap()
    {
        if (nailSlotTransform != null && Vector3.Distance(transform.position, nailSlotTransform.position) <= snapRange)
        {
            SnapToPosition(nailSlotTransform);
            Debug.Log("SNAPPIN");
        }
    }

    void SnapToPosition(Transform targetTransform)
    {
        // Snap the object to the target position
        Vector3 snapPosition = targetTransform.position;
        snapPosition.y = transform.position.y; // Keep the original Y position
        transform.position = snapPosition;
        transform.rotation = targetTransform.rotation;
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