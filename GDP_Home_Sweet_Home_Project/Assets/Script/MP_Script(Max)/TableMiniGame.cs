using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableMiniGame : MonoBehaviour
{
    private Vector3 offset;
    private bool isDragging = false;
    private bool isAttached = false;
    private Rigidbody rb;
    public LayerMask groundLayerMask;
    public float hoverHeight = 0.4f;
    private bool hittingPos = false;

    [SerializeField] private string targetSlot;

    private Vector3 originalPos;
    private Quaternion originalRotation;

    public AudioSource audioSource;
    public AudioClip snapSound;

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

    void OnMouseUp()
    {
        Debug.Log("NOT DRAGGING");
        isDragging = false;
        CheckForSnap();
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

    void CheckForSnap()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag(targetSlot) && !isAttached)
            {
                SnapToPosition(col);
                break;
            }
        }
    }

    void SnapToPosition(Collider snapCollider)
    {
        Vector3 snapPosition = snapCollider.transform.position;
        snapPosition.y += 1.8f;
        transform.position = snapPosition;
        transform.rotation = Quaternion.identity * Quaternion.Euler(180, 0, 0);
        transform.SetParent(snapCollider.transform);

        // Check and destroy any other child objects with the "SnapPosition" tag
        GameObject parentObject = this.transform.parent.gameObject;
        foreach (Transform child in parentObject.transform)
        {
            if (child.CompareTag("SnapPosition"))
            {
                Destroy(child.gameObject);
                break;
            }
        }

        audioSource.PlayOneShot(snapSound);
        rb.isKinematic = true;
        isAttached = true;
        EnableAllChildren();
        StartCoroutine(DeleteWithDelay());
        ShelfMiniGameManager.Instance.IncrementMountCount();
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

    private void OnDrawGizmos()
    {
        if (isDragging)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, transform.localScale);
        }
    }
}