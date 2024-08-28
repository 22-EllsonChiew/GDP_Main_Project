using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapCollider : MonoBehaviour
{
    //public GameObject player;
    public MovingFurniture movingFurniture;

    private Collider snapCollider;

    private MeshRenderer meshRenederer;
    public GameObject disableObjectRender;
    public string objectTag;


    [Header("Cupboard")]
    public Vector3 prefabRotation = new Vector3(90, 0, 181);
    public Vector3 prefabPosition = new Vector3(3.636f, 0.79f, -58.5f);
    public GameObject cupBoardPrefab;

    [Header("Mirror")]
    public Vector3 mirrorPrefabRotation = new Vector3(0,0,0);
    public Vector3 mirrorPrefabPosition = new Vector3(0, 0, 0);
    public GameObject mirrorPrefab;

    [Header("Bar Stool")]
    public Vector3 barStoolPrefabPosition = new Vector3(0, 0, 0);
    public Vector3 barStoolPrefabRotation = new Vector3(0, 0, 0);
    public GameObject barStoolPrefab1;
    public GameObject barStoolTranslucent;

    [Header("Tv Table")]
    public Vector3 tvTablePrefabPosition = new Vector3(0, 0, 0);
    public Vector3 tvTablePrefabRotation = new Vector3(0, 0, 0);
    public GameObject tvTablePrefab;
    public GameObject translucentTVSet;

    [Header("Study Table")]
    public Vector3 studyTablePrefabPosition = new Vector3(0, 0, 0);
    public Vector3 studyTablePrefabRotation = new Vector3(0, 0, 0);
    public GameObject studyTablePrefab;
    public GameObject translucentStudyTable;

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

    private readonly HashSet<string> draggingTags = new HashSet<string>
    {
        "Object", "Drilling", "Draggable", "DraggableMirror", "DraggableBarStool", "DraggableTvTable", "DraggableStudyTable"
    };

    private void OnTriggerStay(Collider other)
    {
        //MovingFurniture movingFurniture = FindObjectOfType<MovingFurniture>();
        if (other.CompareTag("Object") || other.CompareTag("Drilling") || other.CompareTag("Draggable") || other.CompareTag("DraggableMirror") || other.CompareTag("DraggableBarStool") || other.CompareTag("DraggableTvTable") || other.CompareTag("DraggableStudyTable"))
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
                    disableObjectRender.SetActive(false);
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
                    barStoolTranslucent.SetActive(false);
                    Quaternion prefabBarStool1Quaternion = Quaternion.Euler(barStoolPrefabRotation);

                    GameObject prebuiltBarStool1 = Instantiate(barStoolPrefab1, barStoolPrefabPosition, prefabBarStool1Quaternion);
                }
                if(other.CompareTag("DraggableTvTable"))
                {
                    other.gameObject.SetActive(false);
                    translucentTVSet.SetActive(false);
                    Quaternion prefabTvTableQua = Quaternion.Euler(tvTablePrefabRotation);
                    GameObject prebuiltTvTable = Instantiate(tvTablePrefab, tvTablePrefabPosition, prefabTvTableQua);
                }
                if(other.CompareTag("DraggableStudyTable"))
                {
                    Debug.Log("DISAPPEAR!");
                    other.gameObject.SetActive(false);
                    translucentStudyTable.SetActive(false);
                    Quaternion prefabStudyTableQua = Quaternion.Euler(studyTablePrefabRotation);
                    GameObject prebuiltStudyTable = Instantiate(studyTablePrefab, studyTablePrefabRotation, prefabStudyTableQua);
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
