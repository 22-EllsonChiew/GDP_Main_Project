using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TableDrillingMiniGame : MonoBehaviour
{
    public int timeNeeded = 5;
    private float currentTimeHeld = 0f;

    public float noiseThreshold = 0.7f;
    private float currentNoise = 0f;

    public float noiseIncreaseRate = 0.1f;
    public float noiseDecreaseRate;

    public bool isMinigameActive = false;

    public UICursor uiCursor;

    public GameObject minigameUI;

    public Slider noise;
    public Slider progress;
    public ParticleSystem hitParticles;

    public Image fill;
    public Gradient gradient;

    public GameObject oldChair;
    public GameObject newChair;

    private DrillingNailController currentNail;

    public AudioSource drillingAudio;
    public AudioClip drillSound;

    public GameObject mainCam;
    public GameObject miniGameCam;
    public Camera camRay;
    public LayerMask nailLayer;

    private Transform newChairPos;

    public UnityEvent<bool> taskCompleted;
    public UnityEvent resetLeg;

    public bool debugBuild = false;
    public GameObject tableObject;

    [Header("Noise Controller")]
    public NoiseController noiseController;
    public WindowController windowController;

    private float maxNoiseIncreaseRate = 0.2f; // Maximum rate of noise increase
    private float holdTime = 0f; // Time the button is held


    void Start()
    {
        miniGameCam.SetActive(false);

        //noise.maxValue = noiseThreshold;

        drillingAudio = GetComponent<AudioSource>();
        drillingAudio.clip = drillSound;

        newChairPos = oldChair.transform;
    }

    void Update()
    {
        if (isMinigameActive)
        {
            Ray ray = camRay.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, nailLayer))
            {
                Cursor.visible = false;
                uiCursor.ShowCursor();

                if (Input.GetMouseButtonDown(0))
                {
                    drillingAudio.Play();
                    holdTime = 0f;

                }

                if (Input.GetMouseButton(0))
                {
                    holdTime += Time.deltaTime;

                    float noiseLevel = Mathf.Lerp(noiseIncreaseRate, maxNoiseIncreaseRate, holdTime);
                    noiseController.MakeNoise(windowController.NoiseLevel());
                    noiseController.HandleNoise();

                    HandleHoldClick();

                }

                if (Input.GetMouseButtonUp(0))
                {
                    drillingAudio.Stop();
                    holdTime = 0f;

                }

                // Update the progress slider
                if (currentNail != null)
                {
                    //progress.value = currentNail.currentProgress;
                }
            }
            else
            {
                uiCursor.HideCursor();
                Cursor.visible = true;
            }

            /*currentNoise = Mathf.Max(0f, currentNoise - noiseDecreaseRate * Time.deltaTime);
            noise.value = (noiseThreshold != 0f) ? currentNoise / noiseThreshold : 0f;
            fill.color = gradient.Evaluate(currentNoise);*/
            //Debug.Log(currentTimeHeld);
            GameObject[] nails = GameObject.FindGameObjectsWithTag("Nail");
            Debug.Log("Nails left = " + nails.Length);

            CheckDebugBuild();
        }
    }

    public void StartMinigame(GameObject nailPrefab)
    {
        if (nailPrefab == null)
        {
            Debug.LogError("nailPrefab is null.");
            return;
        }

        noiseDecreaseRate = noiseIncreaseRate * 2.25f;
        isMinigameActive = true;
        currentNail = nailPrefab.GetComponent<DrillingNailController>();
        minigameUI.SetActive(true);

        miniGameCam.SetActive(true);
        mainCam.SetActive(false);


        Debug.Log("Minigame started");

        // Set the max value of the progress slider
        progress.maxValue = timeNeeded;
    }

    public void EndMinigame()
    {
        drillingAudio.Stop();
        isMinigameActive = false;
        minigameUI.SetActive(false);

        if (currentNail != null)
        {
            Destroy(currentNail.gameObject);
        }

        currentNail = null;
        Cursor.visible = true;

        Debug.Log("Minigame ended");

        //miniGameCam.SetActive(false);
        //mainCam.SetActive(true);

        // Check if all nails are drilled in
        GameObject[] nails = GameObject.FindGameObjectsWithTag("Nail");
        if (nails.Length == 1)
        {
            BuildObject();
        }
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
        //StartCoroutine(DestroyDelay());
        ScoreManager.Instance.IncrementTotalFunitureCount();
        StartCoroutine(RotatingNew());
        taskCompleted.Invoke(true);
    }

    IEnumerator RotatingNew()
    {
        //yield return new WaitForSeconds(2f);
        //determining rotation for object
        Quaternion targetRotation = Quaternion.Euler(-36.897f, 0f, 0f);
        //determining position for object
        Vector3 startPosition = tableObject.transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * 1f;
        float elapsedTime = 0f;
        float rotationTime = 3f;

        while (elapsedTime < rotationTime)
        {
            //lerp rotation and position of shelfObject
            tableObject.transform.rotation = Quaternion.Lerp(tableObject.transform.rotation, targetRotation, elapsedTime / rotationTime);
            tableObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        miniGameCam.SetActive(false);
        mainCam.SetActive(true);
    }

    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(1.25f);

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, 180f);
        float elapsedTime = 0f;
        float rotationTime = 2f;

        while (elapsedTime < rotationTime)
        {
            oldChair.transform.rotation = Quaternion.Lerp(oldChair.transform.rotation, targetRotation, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        oldChair.SetActive(false);

        var instantiatedChair = Instantiate(newChair, new Vector3(newChairPos.position.x, newChairPos.position.y, newChairPos.position.z), transform.rotation);

        yield return new WaitForSeconds(2f);

        Destroy(instantiatedChair);

        oldChair.SetActive(true);
        oldChair.transform.rotation = Quaternion.identity;

        resetLeg.Invoke();
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