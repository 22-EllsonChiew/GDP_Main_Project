using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFurniture : MonoBehaviour
{
    public GameObject player;
    public Transform carryPos;
    //radius to check for draggable objects
    public float checkRadius = 2f;
    //currently carried object
    private GameObject carriedObject = null;
    private Quaternion initialRotation;

    // Update is called once per frame
    void Update()
    {
        CheckForDraggableObject();
        CarryObject();
    }

    void CheckForDraggableObject()
    {
        //check if player presses the G key
        if (Input.GetKeyDown(KeyCode.G))
        {
            //if already carrying an object, drop it
            if (carriedObject != null)
            {
                DropObject();
                return;
            }

            //find for object in radius using overlapsphere
            Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, checkRadius);
            foreach (var hitCollider in hitColliders)
            {
                //check for tag "Draggable"
                if (hitCollider.CompareTag("Draggable"))
                {
                    //set carriedObject to collided object
                    carriedObject = hitCollider.gameObject;
                    //set object to carryPos
                    carriedObject.transform.position = carryPos.position;
                    //lock rotation of object to carryPos rotation
                    //carriedObject.transform.rotation = carryPos.rotation;
                    //rotation of object is always based on the foward direction of the player
                    Vector3 forwardDirection = player.transform.forward;
                    forwardDirection.y = 0; // Keep it upright
                    carriedObject.transform.rotation = Quaternion.LookRotation(forwardDirection);
                    //object made as child of carryPos
                    carriedObject.transform.SetParent(carryPos); 
                    break;
                }
            }
        }
    }

    void CarryObject()
    {
        //keep updating position of object to stay in front of the player
        if (carriedObject != null)
        {
            //object follows carryPos position and player rotation
            carriedObject.transform.position = carryPos.position;
            carriedObject.transform.rotation = carryPos.rotation;
        }
    }

    void DropObject()
    {
        //remove the parent and set the carried object to null
        carriedObject.transform.SetParent(null);
        carriedObject = null;
    }
}
