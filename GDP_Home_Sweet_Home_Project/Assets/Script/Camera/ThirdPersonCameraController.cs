using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{

    public NeighbourUIController neighbourUI;

    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float heightValue;
    [SerializeField] private Vector3 playerRotationOffset;



    
    public float pLerp = 1f;
    public float rLerp = .04f;
    
    
    public float lerpDuration = 1f;

    private bool neighbourEndConvo = false;



    [Header("Neighbour")]
    public BoxCollider SherrylNeighbourBox;
    public BoxCollider HakimNeighbourBox;

    public Transform SherrylCamHolder;
    public Transform HakimCamHolder;

    private bool playerInSherrylNeighbourBox;
    private bool playerInHakimNeighbourBox;

    private bool learpToCamHolder = false;

    [SerializeField] private Vector3 HakimneighbourOffSet;
    [SerializeField] private float neighbourHeighValue;

    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private Vector3 sherrylOffset;

    private Transform targetPos;

    // Update is called once per frame

    void Start()
    {
        
    }
    void Update()
    {
        playerInSherrylNeighbourBox = SherrylNeighbourBox.bounds.Contains(player.position);
        playerInHakimNeighbourBox = HakimNeighbourBox.bounds.Contains(player.position);

        if ((playerInSherrylNeighbourBox || playerInHakimNeighbourBox) && Input.GetKeyDown(KeyCode.E))
        {
            //learpToCamHolder = true;
            if (playerInSherrylNeighbourBox)
            {
                
                targetPos = SherrylCamHolder;
            }
            if (playerInHakimNeighbourBox)
            {
                
                targetPos = HakimCamHolder;
            }

        }

        if(neighbourUI.endInteraction == true)
        {
            neighbourEndConvo = true;
            Debug.Log("Neighbour shut up");
        }
    }

    private void LateUpdate()
    {
        //set the position of the player to follow
        transform.position = player.position - offset + Vector3.up * heightValue; 
        //set the rotation of the camera, can be change in the inspector
        transform.rotation = Quaternion.Euler(playerRotationOffset); 

        //check if the player is inside the neighbour box collider if it is, it will give the snapping effect
        if (playerInSherrylNeighbourBox)
        {
            //set the camera to the position of sherryl camera holder position
            transform.position = SherrylCamHolder.position - sherrylOffset + Vector3.up * neighbourHeighValue;
            //transform.rotation = SherrylCamHolder.rotation * rotationOffset;
            transform.rotation = Quaternion.Euler(rotationOffset); //set the rotation of the camera, can be change in the inspector


        }
        else if (playerInHakimNeighbourBox)
        {
            //set the camera to the position of hakim camera holder position
            transform.position = HakimCamHolder.position - HakimneighbourOffSet + Vector3.up * neighbourHeighValue;
            //transform.rotation = HakimCamHolder.rotation * rotationOffset;
            transform.rotation = Quaternion.Euler(rotationOffset); //set the rotation of the camera, can be change in the inspector
        }


        else if (neighbourEndConvo)
        {

            transform.position = Vector3.Lerp(transform.position, player.position - offset + Vector3.up * heightValue, pLerp);
            transform.rotation = Quaternion.Euler(playerRotationOffset);


        }
        
    
        //transform.position = player.position - offset + Vector3.up * heightValue;

    }
}
