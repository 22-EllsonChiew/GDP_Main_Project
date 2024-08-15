using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapCollider : MonoBehaviour
{
    //public GameObject player;
    public MovingFurniture movingFurniture;

    public GameObject cupBoardPrefab;

    private Collider snapCollider;

    private MeshRenderer meshRenederer;

    public Vector3 prefabRotation = new Vector3(90, 0, 181);

    private void Start()
    {
        snapCollider = GetComponent<Collider>();
        meshRenederer = GetComponent<MeshRenderer>();

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
        if (other.CompareTag("Object") || other.CompareTag("Drilling") || other.CompareTag("Draggable"))
        {
            Debug.Log("Hit Object or Drilling");
            //other.transform.position = this.transform.position;
            //other.transform.rotation = this.transform.rotation;
            //movingFurniture.canSnap = false; // Reset canSnap to avoid multiple snapping
            if (movingFurniture != null && movingFurniture.canSnap)
            {
                other.transform.position = this.transform.position;
                other.transform.rotation = this.transform.rotation;

                

                other.gameObject.SetActive(false);
                Quaternion prefabQuaternion = Quaternion.Euler(prefabRotation);

                GameObject preBuiltFurniture = Instantiate(cupBoardPrefab, this.transform.position, prefabQuaternion);

                snapCollider.enabled = false;

                if(meshRenederer != null)
                {
                    meshRenederer.enabled = false;
                }


                movingFurniture.canSnap = false; // Reset canSnap to avoid multiple snapping
            }
        }
    }
}
