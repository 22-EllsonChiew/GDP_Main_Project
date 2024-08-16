using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapCollider : MonoBehaviour
{
    //public GameObject player;
    public MovingFurniture movingFurniture;

    private Collider snapCollider;

    private MeshRenderer meshRenederer;


    [Header("Cupboard")]
    public Vector3 prefabRotation = new Vector3(90, 0, 181);
    public Vector3 prefabPosition = new Vector3(3.636f, 0.771f, -58.5f);
    public GameObject cupBoardPrefab;

    [Header("Mirror")]
    public Vector3 mirrorPrefabRotation = new Vector3(0,0,0);
    public Vector3 mirrorPrefabPosition = new Vector3(0, 0, 0);
    public GameObject mirrorPrefab;

    [Header("Bar Stool")]
    public Vector3 barStoolPrefabPosition = new Vector3(0, 0, 0);
    public Vector3 barStoolPrefabRotation = new Vector3(0, 0, 0);
    public GameObject barStoolPrefab1;

    [Header("Tv Table")]
    public Vector3 tvTablePrefabPosition = new Vector3(0, 0, 0);
    public Vector3 tvTablePrefabRotation = new Vector3(0, 0, 0);
    public GameObject tvTablePrefab;

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
        if (other.CompareTag("Object") || other.CompareTag("Drilling") || other.CompareTag("Draggable") || other.CompareTag("DraggableMirror") || other.CompareTag("DraggableBarStool") || other.CompareTag("DraggableTvTable"))
        {
            Debug.Log("Hit Object or Drilling");
            //other.transform.position = this.transform.position;
            //other.transform.rotation = this.transform.rotation;
            //movingFurniture.canSnap = false; // Reset canSnap to avoid multiple snapping
            if (movingFurniture != null && movingFurniture.canSnap)
            {
                other.transform.position = this.transform.position;
                other.transform.rotation = this.transform.rotation;

                
                if(other.CompareTag("Draggable"))
                {
                    other.gameObject.SetActive(false);
                    Quaternion prefabQuaternion = Quaternion.Euler(prefabRotation);

                    GameObject preBuiltFurniture = Instantiate(cupBoardPrefab, prefabPosition, prefabQuaternion);
                }
                if(other.CompareTag("DraggableMirror"))
                {
                    other.gameObject.SetActive(false);

                    Quaternion mirrorPrefabQuaternion = Quaternion.Euler(mirrorPrefabRotation);

                    GameObject preBuiltmirrorPrefab = Instantiate(mirrorPrefab, mirrorPrefabPosition, mirrorPrefabQuaternion);

                }
                if(other.CompareTag("DraggableBarStool"))
                {
                    other.gameObject.SetActive(false);
                    Quaternion prefabBarStool1Quaternion = Quaternion.Euler(barStoolPrefabRotation);

                    GameObject prebuiltBarStool1 = Instantiate(barStoolPrefab1, barStoolPrefabPosition, prefabBarStool1Quaternion);
                }
                if(other.CompareTag("DraggableTvTable"))
                {
                    other.gameObject.SetActive(false);
                    Quaternion prefabTvTableQua = Quaternion.Euler(tvTablePrefabRotation);
                    GameObject prebuiltTvTable = Instantiate(tvTablePrefab, tvTablePrefabPosition, prefabTvTableQua);
                }
                
                

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
