using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{

    public NeighbourUIController neighbourUI;

    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float heightValue;



    
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

    [SerializeField] private Vector3 neighbourOffSet;
    [SerializeField] private float neighbourHeighValue;

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
            learpToCamHolder = true;
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
        transform.position = player.position - offset + Vector3.up * heightValue;

        if (learpToCamHolder && playerInSherrylNeighbourBox)
        {
            //transform.position = Vector3.Lerp(transform.position, targetPos.position, pLerp);
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetPos.rotation, rLerp);

            transform.position = SherrylCamHolder.position - neighbourOffSet + Vector3.up * neighbourHeighValue;


        }
        else if (learpToCamHolder && playerInHakimNeighbourBox)
        {
            transform.position = HakimCamHolder.position - neighbourOffSet + Vector3.up * neighbourHeighValue;
        }



        else if (neighbourEndConvo)
        {

            transform.position = Vector3.Lerp(transform.position, player.position - offset + Vector3.up * heightValue, pLerp);


        }
        
    
        //transform.position = player.position - offset + Vector3.up * heightValue;

    }
}
