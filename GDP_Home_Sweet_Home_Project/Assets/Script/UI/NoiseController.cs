using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class NoiseController : MonoBehaviour
{
    [Header("Noise Parameters")]
    private float currentNoise;
    private float noiseThreshold = 1f;
    public float noiseDecreaseRate;



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

        if (playerInSherrylSide && windowControllers.leftWindowIsClose())
        {
            if(Input.GetKeyUp(KeyCode.E))
            {
                MakeNoise(0.25f);
                HandleNoise();
            }
            
        }
        else if(playerInSherrylSide && Input.GetKeyUp(KeyCode.E))
        {
            MakeNoise(0.55f);
            HandleNoise();
        }
        

        if(playerInHakimSide && windowControllers.rightWindowIsClose())
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                MakeNoise(0.25f);
                HandleNoise();
            }
        }
        else if (playerInHakimSide && Input.GetKeyUp(KeyCode.E))
        {
            MakeNoise(0.55f);
            HandleNoise();
        }
        

    }

    void UpdateBar()
    {
        currentNoise = Mathf.Max(0f, currentNoise - noiseDecreaseRate * Time.deltaTime);

        noiseBar.value = (noiseThreshold != 0f) ? currentNoise / noiseThreshold : 0f;

        noiseColourFill.color = noiseGradient.Evaluate(currentNoise);
    }

    void HandleNoise()
    {
        if (currentNoise > 0.50f)
        {
            if(playerInHakimSide)
            {
                neighbourHakim.HeardNoise(noiseDecreaseRate);
            }
            else if(playerInSherrylSide)
            {
                neighbourSheryl.HeardNoise(noiseDecreaseRate);
            }
        }
    }

    public void MakeNoise(float noise)
    {
        currentNoise = Mathf.Min(currentNoise + noise, noiseThreshold);
    }

    public void NeighbourBoxCollider()
    {
        playerInSherrylSide = sherylSideCollider.bounds.Contains(player.position);
        playerInHakimSide = HakimSideCollider.bounds.Contains(player.position);
    }
}
