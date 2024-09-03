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
    public GameObject barStoolTranslucent2;

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

    [Header("Office Chair")]
    public Vector3 officeChairPrefabPosition = new Vector3(0, 0, 0);
    public Vector3 officeChairPrefabRotation = new Vector3(0, 0, 0);
    public GameObject officeChairPrefab;
    public GameObject translucentOfiiceChair;

    [Header("Sofa")]
    public Vector3 sofaPrefabPosition = new Vector3(0, 0, 0);
    public Vector3 sofaPrefabRotation = new Vector3(0, 0, 0);
    public GameObject sofaPrefab;
    public GameObject TranslucentSofa;

    [Header("Living Room Lamp")]
    public Vector3 lampPrefabPosition = new Vector3(0, 0, 0);
    public Vector3 lampPrefabRotation = new Vector3(0, 0, 0);
    public GameObject lampPrefab;
    public GameObject TranslucentLamp;

    [Header("Bed Room Lamp")]
    public Vector3 bedRoomLampPosition = new Vector3(0, 0, 0);
    public Vector3 bedRoomLampRotation = new Vector3(0, 0, 0);
    public GameObject bedRoomLampPrefab;
    public GameObject TranslucentBedRoomLamp;

    [Header("Dining Chair 1")]
    public Vector3 diningChair1Pos = new Vector3(0, 0, 0);
    public Vector3 diningChair1Rota = new Vector3(0, 0, 0);
    public GameObject diningChair1Prefab;
    public GameObject TranslucentDc1Prefab;

    [Header("Dining Chair 2")]
    public Vector3 diningChair2Pos = new Vector3(0, 0, 0);
    public Vector3 diningChair2Rota = new Vector3(0, 0, 0);
    public GameObject diningChair2Prefab;
    public GameObject TranslucentDc2Prefab;

    [Header("Dining Chair 3")]
    public Vector3 diningChair3Pos = new Vector3(0, 0, 0);
    public Vector3 diningChair3Rota = new Vector3(0, 0, 0);
    public GameObject diningChair3Prefab;
    public GameObject TranslucentDc3Prefab;

    [Header("Dining Chair 4")]
    public Vector3 diningChair4Pos = new Vector3(0, 0, 0);
    public Vector3 diningChair4Rota = new Vector3(0, 0, 0);
    public GameObject diningChair4Prefab;
    public GameObject TranslucentDc4Prefab;

    [Header("Dining Table")]
    public Vector3 diningTablePos = new Vector3(0, 0, 0);
    public Vector3 diningTableRota = new Vector3(0, 0, 0);
    public GameObject diningTablePrefab;
    public GameObject TranslucenDiningTablePrefab;

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
        "Object", "Drilling", "Draggable", "DraggableMirror", "DraggableBarStool", "DraggableTvTable", "DraggableStudyTable", "DraggableBarStool2", "DraggableOfficeChair", "DraggableSofa", "DraggableLRLamp", "DraggableBRLamp", "DraggableDiningChair",
        "DraggableDC2", "DraggableDC3", "DraggableDC4", "DraggableDiningTable"
     };

    private void OnTriggerStay(Collider other)
    {
        //MovingFurniture movingFurniture = FindObjectOfType<MovingFurniture>();
        if (draggingTags.TryGetValue(other.tag, out _))
        {
            Debug.Log("Hit Object or Drilling");
            //other.transform.position = this.transform.position;
            //other.transform.rotation = this.transform.rotation;
            //movingFurniture.canSnap = false; // Reset canSnap to avoid multiple snapping
            if (movingFurniture != null && movingFurniture.canSnap)
            {
                other.transform.position = this.transform.position;
                other.transform.rotation = this.transform.rotation;


                /*if(other.CompareTag("Draggable"))
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
                    
                    other.gameObject.SetActive(false);
                    translucentStudyTable.SetActive(false);
                    Quaternion prefabStudyTableQua = Quaternion.Euler(studyTablePrefabRotation);
                    GameObject prebuiltStudyTable = Instantiate(studyTablePrefab, studyTablePrefabPosition, prefabStudyTableQua);
                }
                if(other.CompareTag("DraggableBarStool2"))
                {
                    other.gameObject.SetActive(false);
                    barStoolTranslucent2.SetActive(false);
                    Quaternion prefabBarStool2Quaternion = Quaternion.Euler(barStoolPrefabRotation);

                    GameObject prebuiltBarStool2 = Instantiate(barStoolPrefab1, barStoolPrefabPosition, prefabBarStool2Quaternion);
                }*/

                ObjectSnapLogic(other);

                snapCollider.enabled = false;

                if(meshRenederer != null)
                {
                    meshRenederer.enabled = false;
                }


                movingFurniture.canSnap = false; // Reset canSnap to avoid multiple snapping
            }
        }
    }

    void ObjectSnapLogic(Collider other)
    {
        switch(other.tag)
        {
            case "Draggable":
                other.gameObject.SetActive(false);
                Quaternion prefabQuaternion = Quaternion.Euler(prefabRotation);
                GameObject preBuiltFurniture = Instantiate(cupBoardPrefab, prefabPosition, prefabQuaternion);
                disableObjectRender.SetActive(false);
                break;
            case "DraggableMirror":
                other.gameObject.SetActive(false);
                Quaternion mirrorPrefabQuaternion = Quaternion.Euler(mirrorPrefabRotation);
                GameObject preBuiltmirrorPrefab = Instantiate(mirrorPrefab, mirrorPrefabPosition, mirrorPrefabQuaternion);
                break;
            case "DraggableBarStool":
                other.gameObject.SetActive(false);
                barStoolTranslucent.SetActive(false);
                Quaternion prefabBarStool1Quaternion = Quaternion.Euler(barStoolPrefabRotation);
                GameObject prebuiltBarStool1 = Instantiate(barStoolPrefab1, barStoolPrefabPosition, prefabBarStool1Quaternion);
                break;
            case "DraggableTvTable":
                other.gameObject.SetActive(false);
                translucentTVSet.SetActive(false);
                Quaternion prefabTvTableQua = Quaternion.Euler(tvTablePrefabRotation);
                GameObject prebuiltTvTable = Instantiate(tvTablePrefab, tvTablePrefabPosition, prefabTvTableQua);
                break;
            case "DraggableBarStool2":
                other.gameObject.SetActive(false);
                barStoolTranslucent2.SetActive(false);
                Quaternion prefabBarStool2Quaternion = Quaternion.Euler(barStoolPrefabRotation);
                GameObject prebuiltBarStool2 = Instantiate(barStoolPrefab1, barStoolPrefabPosition, prefabBarStool2Quaternion);
                break;
            case "DraggableOfficeChair":
                other.gameObject.SetActive(false);
                translucentOfiiceChair.SetActive(false);
                Quaternion officeChairQuaternion = Quaternion.Euler(officeChairPrefabRotation);
                GameObject officeChairPrebuilt = Instantiate(officeChairPrefab, officeChairPrefabPosition, officeChairQuaternion);
                break;
            case "DraggableSofa":
                other.gameObject.SetActive(false);
                TranslucentSofa.SetActive(false);
                Quaternion sofaQuaternion = Quaternion.Euler(sofaPrefabRotation);
                GameObject sofaPrebuilt = Instantiate(sofaPrefab, sofaPrefabPosition, sofaQuaternion);
                break;
            case "DraggableLRLamp":
                other.gameObject.SetActive(false);
                TranslucentLamp.SetActive(false);
                Quaternion LRLampQuaternion = Quaternion.Euler(lampPrefabRotation);
                GameObject LRLampPrebuilt = Instantiate(lampPrefab, lampPrefabPosition, LRLampQuaternion);
                break;
            case "DraggableBRLamp":
                other.gameObject.SetActive(false);
                TranslucentBedRoomLamp.SetActive(false);
                Quaternion BRLampQuaternion = Quaternion.Euler(bedRoomLampRotation);
                GameObject BRLampPrebuilt = Instantiate(bedRoomLampPrefab, bedRoomLampPosition, BRLampQuaternion);
                break;
            case "DraggableDiningChair":
                other.gameObject.SetActive(false);
                TranslucentDc1Prefab.SetActive(false);
                Quaternion DC1PrefabQua = Quaternion.Euler(diningChair1Rota);
                GameObject DC1Prebuilt = Instantiate(diningChair1Prefab, diningChair1Pos, DC1PrefabQua);
                break;
            case "DraggableDC2":
                other.gameObject.SetActive(false);
                TranslucentDc2Prefab.SetActive(false);
                Quaternion DC2PrefabQua = Quaternion.Euler(diningChair2Rota);
                GameObject DC2Prebuilt = Instantiate(diningChair2Prefab, diningChair2Pos, DC2PrefabQua);
                break;
            case "DraggableDC3":
                other.gameObject.SetActive(false);
                TranslucentDc3Prefab.SetActive(false);
                Quaternion DC3PrefabQua = Quaternion.Euler(diningChair3Rota);
                GameObject DC3Prebuilt = Instantiate(diningChair3Prefab, diningChair3Pos, DC3PrefabQua);
                break;
            case "DraggableDC4":
                other.gameObject.SetActive(false);
                TranslucentDc4Prefab.SetActive(false);
                Quaternion DC4PrefabQua = Quaternion.Euler(diningChair4Rota);
                GameObject DC4Prebuilt = Instantiate(diningChair4Prefab, diningChair4Pos, DC4PrefabQua);
                break;
            case "DraggableDiningTable":
                other.gameObject.SetActive(false);
                TranslucenDiningTablePrefab.SetActive(false);
                Quaternion DiningTableQua = Quaternion.Euler(diningTableRota);
                GameObject DiningTablePreBuilt = Instantiate(diningTablePrefab, diningTablePos, DiningTableQua);
                break;
        }
    }
}
