using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfMiniGame : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private bool isAttached = false;
    private Rigidbody rb;
    public LayerMask groundLayerMask;
    public float hoverHeight = 0.4f;

    [SerializeField] private string targetSlot;

    private Vector3 originalPos;
    private Quaternion originalRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        DisableAllChildren();

        originalPos = transform.position;
        originalRotation = transform.rotation;
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
            Vector3 newPosition = GetMouseWorldPosition() + offset;
            newPosition.y = 0f;

            Ray ray = ShelfMiniGameManager.Instance.gameCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayerMask))
            {
                newPosition.y = hit.point.y + hoverHeight;
            }

            transform.position = newPosition;
            transform.rotation = Quaternion.identity;
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = ShelfMiniGameManager.Instance.gameCamera.transform.position.y;
        return ShelfMiniGameManager.Instance.gameCamera.ScreenToWorldPoint(mousePosition);
    }

    void OnMouseUp()
    {
        isDragging = false;
        SnapToTrigger();
    }

    void SnapToTrigger()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f);

        foreach (Collider col in colliders)
        {
            if (col.isTrigger && !isAttached)
            {
                if (col.gameObject.CompareTag(targetSlot))
                {
                    Vector3 snapPosition = col.transform.position;
                    transform.position = snapPosition;
                    transform.rotation = Quaternion.identity * Quaternion.Euler(110, 0, 0);
                    transform.SetParent(col.transform);
                    rb.isKinematic = true;
                    isAttached = true;
                    EnableAllChildren();
                    StartCoroutine(DeleteWithDelay());
                    ShelfMiniGameManager.Instance.IncrementMountCount();
                    break;
                }
            }
            else
            {
                if (!isAttached)
                {
                    rb.isKinematic = false;
                }
            }
        }
    }

    public void ResetObject()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        isAttached = false;
        transform.position = originalPos;
        transform.rotation = originalRotation;
    }

    void DisableAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    void EnableAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    IEnumerator DeleteWithDelay()
    {
        yield return new WaitForSeconds(1.5f);
    }
}