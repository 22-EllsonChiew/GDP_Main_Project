using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseController : MonoBehaviour
{
    [Header("Noise Parameters")]
    private float currentNoise;
    public float noiseThreshold = 100f;
    public float noiseDecreaseRate;
    private float noiseMultiplier = 0.001f;



    [Header("UI Elements")]
    public Slider noiseBar;
    public Image noiseColourFill;
    public Gradient noiseGradient;


    [Header("Neighbours")]
    [SerializeField] private Transform player;
    public NeighbourAngerBar neighbourSheryl;
    public NeighbourAngerBar neighbourHakim;

    public BoxCollider sherylSideCollider;
    public BoxCollider HakimSideCollider;

    private bool playerInSherrylSide;
    private bool playerInHakimSide;

    [Header("Player Inside Collider")]
    
    [SerializeField] private GameObject closedWindow;

    [Header("Scripts")]

    public WindowController windowControllers;
    //public NailGame nailGameController;
    public PlayerMovement playerMovementController;
    public MovingFurniture movingFurnitureController;
    public PackageRaycast packageRaycast;

    public static NoiseController instance;

    // Start is called before the first frame update
    void Start()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBar();

        NeighbourBoxCollider();

        // handle noise here
        // make sure that neighbour affected is based on the side of home player is making noise
        // 

        if (Input.GetKeyDown(KeyCode.F))
        {
            CheckIfPlayerInNeighboursSides();
            //CheckIfPackageOnCarpet();
            HandleNoise();
        }




    }

    void UpdateBar()
    {
        currentNoise = Mathf.Max(0f, currentNoise - noiseDecreaseRate * Time.deltaTime);

        noiseBar.value = (noiseThreshold != 0f) ? currentNoise / noiseThreshold : 0f;

        noiseColourFill.color = noiseGradient.Evaluate(currentNoise);
    }

    public void HandleNoise()
    {
        if (currentNoise > 0.60f)
        {
            if(playerInHakimSide)
            {
                neighbourHakim.HeardNoise(noiseDecreaseRate);
            }
            if(playerInSherrylSide)
            {
                neighbourSheryl.HeardNoise(noiseDecreaseRate);
            }
        }
    }

    public void MakeNoise(float noise)
    {
        if(playerMovementController.isWalking && movingFurnitureController.carriedObject != null)
        {
            noise += playerMovementController.speed * noiseMultiplier;
        }

        currentNoise = Mathf.Min(currentNoise + noise, noiseThreshold);
    }

    public void NeighbourBoxCollider()
    {
        playerInSherrylSide = sherylSideCollider.bounds.Contains(player.position);
        playerInHakimSide = HakimSideCollider.bounds.Contains(player.position);
    }

    public void CheckIfPlayerInNeighboursSides()
    {
        if(playerInSherrylSide)
        {
            //MakeNoise(windowControllers.leftWindowIsClose() ? 0.25f : 0.55f);
            MakeNoise(windowControllers.NoiseLevel());
        }
        else if(playerInHakimSide)
        {
            //MakeNoise(windowControllers.rightWindowIsClose() ? 0.25f : 0.55f);
            MakeNoise(windowControllers.NoiseLevel());
        }
    }

    public void CheckIfPackageOnCarpet()
    {
        if(playerInSherrylSide)
        {
            MakeNoise(packageRaycast.OnCarpet() ? 0.35f : 0.55f);
        }
        else if(playerInHakimSide)
        {
            MakeNoise(packageRaycast.OnCarpet() ? 0.35f : 0.55f);
        }
    }
}
