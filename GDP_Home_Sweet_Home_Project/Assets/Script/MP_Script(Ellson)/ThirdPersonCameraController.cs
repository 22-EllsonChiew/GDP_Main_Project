using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{

    [Header("UI References")]
    public NeighbourUIController neighbourUI;
    public GameObject bulletinBoardUI;


    [Header("Player Camera")]
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float heightValue;
    [SerializeField] private Vector3 playerRotationOffset;

    [SerializeField] private Transform playerSecondaryCamHolder;
    [SerializeField] private Vector3 secondaryOffset;
    [SerializeField] private Vector3 playerSecondaryRotationOffset;
    [SerializeField] private float heightValueSecondary;

    [SerializeField] private Transform elevatorCamHolder;
    [SerializeField] private Vector3 elevatorOffset;
    [SerializeField] private Vector3 elevatorRotationOffset;

    private bool playerWalkOutOfHouse = false;
    private bool playerInsideHouse = true;
    private bool playerAtBulletinBoard = false;
    private bool playerInHouseCamOn = false;
    private bool playerAtElevator = false;

    public bool checkBulletinBoard = false;

    public BoxCollider playerCorridor;
    public BoxCollider playerHousing;
    public BoxCollider elevatorCorridor;
    public BoxCollider bulletinBoard;




    public float pLerp = 0.5f;
    public float rLerp = .04f;
    
    
    public float lerpDuration = 1f;

    private bool neighbourEndConvo = false;

    public float cameraForwardOffset;

    

    [Header("Neighbour")]
    public BoxCollider SherrylNeighbourBox;
    public BoxCollider HakimNeighbourBox;

    public Transform SherrylCamHolder;
    public Transform HakimCamHolder;

    public Transform sherrylOutsideCamHolder;
    public Transform HakimOutsideCamHolder;

    private bool playerInSherrylNeighbourBox;
    private bool playerInHakimNeighbourBox;

    private bool learpToCamHolder = false;

    private bool isInsideSherrylCollider = false;
    private bool isInsidHakimCollider = false;

    private bool goToHakimCamHolder = false;
    private bool goToSherrylCamHolder = false;

    [SerializeField] private Vector3 HakimneighbourOffSet;
    [SerializeField] private float neighbourHeighValue;

    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private Vector3 sherrylOffset;


    [SerializeField] private Vector3 HakimOutsideCamHolderOffset;
    [SerializeField] private Vector3 SherrylOutsideCamHolderOffset;
    [SerializeField] private Vector3 rotationOutsideOffset;

    private Transform targetPos;

    // Update is called once per frame

    void Start()
    {
        
    }
    void Update()
    {
        BoxCollider();

        UpdatePlayerIsInNeighbourCollider();

        if (neighbourUI.endInteraction == true)
        {
            
            neighbourEndConvo = true;
            cameraForwardOffset = 0f;

        }
        if (playerAtBulletinBoard && Input.GetKeyDown(KeyCode.E))
        {
            cameraForwardOffset = 2f;
            bulletinBoardUI.SetActive(true);
            player.gameObject.SetActive(false);
            checkBulletinBoard = true;
        }
    }

    private void LateUpdate()
    {
        CorridorCameraFunction();
    }

    private void UpdatePlayerIsInNeighbourCollider()
    {
        BoxCollider();
        if(PlayerInNeighbourCollider() && Input.GetKeyDown(KeyCode.E))
        {
            UpdateCamOffSet();
        }
        
    }

    private void UpdateCamOffSet()
    {
        if (playerInSherrylNeighbourBox)
        {

            targetPos = SherrylCamHolder;
            cameraForwardOffset = 2f;
        }
        if (playerInHakimNeighbourBox)
        {

            targetPos = HakimCamHolder;
            cameraForwardOffset = 2f;
            
        }
       
    }

    private bool PlayerInNeighbourCollider()
    {
        return playerInSherrylNeighbourBox || playerInHakimNeighbourBox;
    }

    public void BoxCollider()
    {
        playerInSherrylNeighbourBox = SherrylNeighbourBox.bounds.Contains(player.position);
        playerInHakimNeighbourBox = HakimNeighbourBox.bounds.Contains(player.position);
        playerWalkOutOfHouse = playerCorridor.bounds.Contains(player.position);
        playerInsideHouse = playerHousing.bounds.Contains(player.position);
        playerAtElevator = elevatorCorridor.bounds.Contains(player.position);
        playerAtBulletinBoard = bulletinBoard.bounds.Contains(player.position);

    }

    public void CorridorCameraFunction()
    {

        //check if the player is inside the neighbour box collider if it is, it will give the snapping effect
        if (playerInSherrylNeighbourBox)
        {
            //set the camera to the position of sherryl camera holder position
            transform.position = Vector3.Lerp(transform.position, SherrylCamHolder.position - sherrylOffset + Vector3.up * neighbourHeighValue + Vector3.forward * cameraForwardOffset, pLerp);
            //transform.rotation = SherrylCamHolder.rotation * rotationOffset;
            transform.rotation = Quaternion.Euler(rotationOffset); //set the rotation of the camera, can be change in the inspector
            checkBulletinBoard = false;
            neighbourUI.endInteraction = false;


        }
        else if (playerInHakimNeighbourBox)
        {
            //set the camera to the position of hakim camera holder position
            transform.position = Vector3.Lerp(transform.position, HakimCamHolder.position - HakimneighbourOffSet + Vector3.up * neighbourHeighValue + Vector3.forward * cameraForwardOffset, pLerp);
            //transform.rotation = HakimCamHolder.rotation * rotationOffset;
            transform.rotation = Quaternion.Euler(rotationOffset); //set the rotation of the camera, can be change in the inspector
            neighbourUI.endInteraction = false;

        }
        else if (checkBulletinBoard)
        {
            transform.position = Vector3.Lerp(transform.position, elevatorCamHolder.position - elevatorOffset + Vector3.up * heightValueSecondary, pLerp);
            transform.rotation = Quaternion.Euler(elevatorRotationOffset);
        }
        else if (playerWalkOutOfHouse || playerAtElevator)
        {
            //transform.position = Vector3.Lerp(transform.position, player.position - secondaryOffset + Vector3.up * heightValueSecondary, pLerp);
            transform.position = player.position - secondaryOffset + Vector3.up * heightValueSecondary;
            transform.rotation = Quaternion.Euler(playerSecondaryRotationOffset);
            playerInsideHouse = false;

            cameraForwardOffset = 0f;
        }
        
        else if (isInsideSherrylCollider == false || isInsidHakimCollider == false || checkBulletinBoard == false)
        {

            transform.position = Vector3.Lerp(transform.position, player.position - offset + Vector3.up * heightValue, pLerp);
            transform.rotation = Quaternion.Euler(playerRotationOffset);

            cameraForwardOffset = 0f;

        }
    }

    public void DisableBulletinBoardUI()
    {
        bulletinBoardUI.SetActive(false);
        player.gameObject.SetActive(true);
        checkBulletinBoard = false;
    }
}