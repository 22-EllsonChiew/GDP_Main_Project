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
                    //set object pos to carryPos
                    carriedObject = hitCollider.gameObject;
                    carriedObject.transform.position = carryPos.position;
                    carriedObject.transform.SetParent(carryPos); // Make the object a child of the carry position
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
            carriedObject.transform.position = carryPos.position;
        }
    }

    void DropObject()
    {
        //remove the parent and set the carried object to null
        carriedObject.transform.SetParent(null);
        carriedObject = null;
    }
}
