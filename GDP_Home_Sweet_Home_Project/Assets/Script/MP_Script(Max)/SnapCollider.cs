using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapCollider : MonoBehaviour
{
    //public GameObject player;
    public MovingFurniture movingFurniture;

    private void Start()
    {
        //movingFurniture = player.GetComponent<MovingFurniture>();
        if (movingFurniture == null)
        {
            Debug.LogError("MovingFurniture component not found");
        }
        else
        {
            Debug.Log("MovingFurniture component assigned");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //MovingFurniture movingFurniture = FindObjectOfType<MovingFurniture>();
        if (other.CompareTag("Object") || other.CompareTag("Drilling"))
        {
            Debug.Log("Hit Object or Drilling");
            //other.transform.position = this.transform.position;
            //other.transform.rotation = this.transform.rotation;
            //movingFurniture.canSnap = false; // Reset canSnap to avoid multiple snapping
            if (movingFurniture != null && movingFurniture.canSnap)
            {
                other.transform.position = this.transform.position;
                other.transform.rotation = this.transform.rotation;
                movingFurniture.canSnap = false; // Reset canSnap to avoid multiple snapping
            }
        }
    }
}
