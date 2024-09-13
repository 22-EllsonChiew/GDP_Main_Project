using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.VisualScripting;

public class RevampedNailGame : MonoBehaviour
{
    public int timeNeeded = 5;
    private float currentTimeHeld = 0f;
    //number of clicks for each nail
    public int clicksNeeded = 5;
    private int currentClicks = 0;
    //number of nails needed to be hit into place
    public int nailsNeeded = 4;
    private int currentNails = 0;

    public float noiseThreshold = 0.7f;
    private float currentNoise = 0f;

    public float noiseIncreaseRate = 0.1f;
    public float noiseDecreaseRate;

    public bool isMinigameActive = false;
    //the hammer sprite cursor
    public UICursor uiCursor;

    public GameObject minigameUI;

    public Slider noise;
    public Slider progress;
    public ParticleSystem hitParticles;

    public Image fill;
    public Gradient gradient;

    private HammerNailController currentNail;

    public AudioSource hammerAudio;
    public AudioClip hammerSound;
    public AudioClip buildComplete;

    public GameObject mainCam;
    public GameObject minigameCam;
    public Camera camRay;
    public LayerMask nailLayer;

    private Transform newChairPos;

    public UnityEvent<bool> taskCompleted;
    public UnityEvent resetLeg;

    public bool debugBuild = false;
    public GameObject chairObject;

    public ScreenShake screenShake;
    public float downwardIncrement = 0.05f;
    public GameObject chairMiniGamePrefab;
    void Start()
    {
        //noise.maxValue = noiseThreshold;

        minigameCam.SetActive(false);

        hammerAudio = GetComponent<AudioSource>();
        hammerAudio.clip = hammerSound;

        //newChairPos = oldChair.transform;
        GameObject minigameCameraObject = GameObject.FindWithTag("MinigameCam");
        if (minigameCameraObject != null)
        {
            screenShake = minigameCameraObject.GetComponent<ScreenShake>();

            if (screenShake == null)
            {
                Debug.LogError("ScreenShake component not found on the MinigameCam.");
            }
        }
       
    }

    private HammerNailController hammerNailController;

    public void Initialize(HammerNailController hammerNailController)
    {
        this.hammerNailController = hammerNailController;
    }

    void Update()
    {
        if (isMinigameActive)
        {
            Ray ray = camRay.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            currentClicks = currentNail.GetComponent<HammerNailController>().currentClicks;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, nailLayer))
            {
                if (uiCursor != null)
                {
                    //Cursor.visible = false;
                    uiCursor.ShowCursor();
                }
                else
                {
                    Debug.LogError("uiCursor is not assigned.");
                }
                if (Input.GetMouseButtonDown(0))
                {
                    screenShake.EnableShake(true);
                    Debug.Log("GOES INTO UPDATE");
                    hammerAudio.PlayOneShot(hammerSound);
                    HandleClick();
                    if (screenShake != null)
                    {
                        screenShake.TriggerShake();
                    }
                }

                // Update the progress slider
                /*if (currentNail != null)
                {
                    progress.value = currentClicks;
                }*/
            }
            else
            {
                uiCursor.HideCursor();
                Cursor.visible = true;
            }

            //currentNoise = Mathf.Max(0f, currentNoise - noiseDecreaseRate * Time.deltaTime);
            //noise.value = (noiseThreshold != 0f) ? currentNoise / noiseThreshold : 0f;
            //fill.color = gradient.Evaluate(currentNoise);
            //Debug.Log(currentTimeHeld);
            GameObject[] nails = GameObject.FindGameObjectsWithTag("Nail");
            Debug.Log("Nails left = " + nails.Length);

            CheckDebugBuild();
        }
    }

    public void StartMinigame(GameObject nailPrefab)
    {
        Debug.Log("START CALL");
        if (nailPrefab == null)
        {
            Debug.LogError("nailPrefab is null.");
            return;
        }

        // Check if the nail prefab is the same as the previous one
        if (currentNail != null && currentNail.gameObject == nailPrefab)
        {
            Debug.Log("Cannot start mini-game with the same nail prefab again.");
            return;
        }

        noiseDecreaseRate = noiseIncreaseRate * 2.25f;
        isMinigameActive = true;

        // Reset currentNail if it's already set
        if (currentNail != null)
        {
            currentNail = null;
        }

        currentNail = nailPrefab.GetComponent<HammerNailController>();
        minigameUI.SetActive(true);

        minigameCam.SetActive(true);
        mainCam.SetActive(false);

        Debug.Log("Minigame started");

        // Set the max value of the progress slider
        //progress.maxValue = clicksNeeded;
    }

    void ResetMinigame()
    {
        isMinigameActive = false;
        currentTimeHeld = 0f;
        currentClicks = 0;
        currentNoise = 0f;
        currentNail = null; // Reset currentNail to null

        // Reset hammerMiniGame to null
        if (hammerNailController != null)
        {
            hammerNailController.hammerMiniGame = null;
        }

        // Hide minigame UI elements
        if (minigameUI != null)
        {
            minigameUI.SetActive(false);
        }

        // Reset camera settings
        if (minigameCam != null)
        {
            minigameCam.SetActive(false);
        }
        if (mainCam != null)
        {
            mainCam.SetActive(true);
        }

        // Reset cursor visibility
        Cursor.visible = true;
    }

    public void EndMinigame()
    {
        hammerAudio.Stop();
        isMinigameActive = false;
        minigameUI.SetActive(false);

        if (currentNail != null)
        {
            Destroy(currentNail.gameObject);
            currentNail = null; // Reset currentNail to null
        }

        // Reset hammerMiniGame to null
        if (hammerNailController != null)
        {
            hammerNailController.hammerMiniGame = null;
        }

        Cursor.visible = true;

        Debug.Log("Minigame ended");

        // Check if all nails are drilled in
        GameObject[] nails = GameObject.FindGameObjectsWithTag("Nail");
        if (currentNails == nailsNeeded)
        {
            BuildObject();
        }
    }

    public void HandleClick()
    {
        Debug.Log("GOES INTO HANDLECLICK()");
        if (currentClicks < clicksNeeded)
        {
            //have the nail go downward a certain amount everytime it is hit
            currentNail.transform.position -= new Vector3(0, downwardIncrement, 0);
            //every click increments currentClicks
            currentNail.GetComponent<HammerNailController>().currentClicks++;
            //acts as an animation for hammering in nails
            StartCoroutine(HammerRotation());
        }
        //when currentClicks is more than or equal to the clicks needed, ends minigame for that nail
        if (currentClicks >= clicksNeeded)
        {
            currentNails++;
            EndMinigame();
        }
    }

    IEnumerator HammerRotation()
    {
        //rotates hammer sprite by 45 to simulate knocking and snap it back to original rotation
        Quaternion originalRotation = uiCursor.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, 45f);
        float timeElapsed = 0f;
        float hammerRotationTime = 0.2f;
        while (timeElapsed < hammerRotationTime)
        {
            timeElapsed += Time.deltaTime;
            uiCursor.transform.rotation = Quaternion.Lerp(uiCursor.transform.rotation, targetRotation, hammerRotationTime / timeElapsed);
            yield return null;
        }
        uiCursor.transform.rotation = originalRotation;
    }

    public void HandleHoldClick()
    {
        currentTimeHeld += Time.deltaTime;
        if (currentNail != null)
        {
            currentNail.currentProgress += Time.deltaTime;
            currentNoise = Mathf.Min(currentNoise + noiseIncreaseRate, noiseThreshold);

            if (currentNail.currentProgress >= timeNeeded)
            {
                Debug.Log("Finished drilling current nail");
                EndMinigame();
            }
        }
    }

    void CheckDebugBuild()
    {
        if (debugBuild)
        {
            BuildObject();
            debugBuild = false; // Reset the debugBuild flag
        }
    }

    void BuildObject()
    {
        Debug.Log("BUILDING");
        ScoreManager.Instance.IncrementTotalFunitureCount();
        hammerAudio.PlayOneShot(buildComplete);

        // Start the coroutine to rotate and replace the chair object
        StartCoroutine(RotatingNew());

        // Trigger any additional events after building
        taskCompleted.Invoke(true);
    }

    IEnumerator RotatingNew()
    {
        // Define the rotation and position for the object
        Quaternion targetRotation = Quaternion.Euler(200f, 0f, 0f);
        Vector3 startPosition = chairObject.transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * 1f;

        // Determine the camera's current and new positions for lerping
        Vector3 currentCamPosition = minigameCam.transform.position;
        Vector3 newCamPosition = currentCamPosition + (Vector3.back * 2f) + (Vector3.up * 1f);
        float elapsedTime = 0f;
        float rotationTime = 3f;

        while (elapsedTime < rotationTime)
        {
            // Lerp rotation and position of chairObject
            chairObject.transform.rotation = Quaternion.Lerp(chairObject.transform.rotation, targetRotation, elapsedTime / rotationTime);
            chairObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;

            // Lerp the position of the camera to move backward
            minigameCam.transform.position = Vector3.Lerp(currentCamPosition, newCamPosition, elapsedTime / rotationTime);
        }

        // Switch cameras back after the rotation is complete
        minigameCam.SetActive(false);
        mainCam.SetActive(true);

        // Store the position and rotation before destroying the current chairObject
        Vector3 spawnPosition = chairObject.transform.position;
        Quaternion spawnRotation = chairObject.transform.rotation;

        // Destroy the current chair object
        Destroy(chairObject);

        // Reset the minigame settings
        ResetMinigame();

        // Instantiate a new chair mini game from the original prefab
        GameObject newChairMiniGame = Instantiate(chairMiniGamePrefab, spawnPosition, spawnRotation);

        // Optionally, initialize or set up the new chair mini game if needed
        // e.g., newChairMiniGame.GetComponent<SomeComponent>().Initialize();
    }

    void OnMouseDown()
    {
        if (!isMinigameActive)
        {
            Ray ray = camRay.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, nailLayer))
            {
                if (hit.collider.CompareTag("Nail"))
                {
                    StartMinigame(hit.collider.gameObject);
                }
            }
        }
    }
}
