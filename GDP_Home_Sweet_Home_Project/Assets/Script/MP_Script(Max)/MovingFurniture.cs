using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFurniture : MonoBehaviour
{
    public GameObject player;
    public PlayerMovement playerMovement;
    //transform in front of player where object will be parented to
    public Transform dragPos;
    //radius to check for draggable objects
    public float checkRadius = 2f; 
    public GameObject carriedObject = null; 
    //height offset from the ground
    public float heightOffset = 0.5f; 

    private void Start()
    {
        //reference player movement script
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        CheckForDraggableObject();
        UpdateCarriedObjectPosition();
    }

    void CheckForDraggableObject()
    {
        //press G to drag object
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (carriedObject != null)
            {
                DropObject();
                return;
            }
            //checks all colliders hit within a sphere around the player's transform
            Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, checkRadius);
            foreach (var hitCollider in hitColliders)
            {
                //if has tag "Draggable" then carried object is set to collided object
                //player speed is also reduced
                if (hitCollider.CompareTag("Draggable"))
                {
                    carriedObject = hitCollider.gameObject;
                    playerMovement.speed = 4f;
                    break;
                }
            }
        }
    }

    void UpdateCarriedObjectPosition()
    {
        if (carriedObject != null)
        {
            //shoots a ray downward and check if it hits
            RaycastHit hit;
            if (Physics.Raycast(dragPos.position, Vector3.down, out hit))
            {
                //wherever the ray hits, set the objects position to the hit point + offset so it does not go into the ground
                Vector3 newPosition = dragPos.position;
                newPosition.y = hit.point.y + heightOffset;
                carriedObject.transform.position = newPosition;
                carriedObject.transform.rotation = Quaternion.LookRotation(player.transform.forward);
            }
        }
    }

    void DropObject()
    {
        //when object is dropped
        if (carriedObject != null)
        {
            //carreid object is not parented to dragPos anymore
            carriedObject.transform.SetParent(null);
            carriedObject = null;
            //player movement speed set back to original speed
            playerMovement.speed = 10f;
        }
    }
}