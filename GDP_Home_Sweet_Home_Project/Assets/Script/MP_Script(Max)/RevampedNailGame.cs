using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class RevampedNailGame : MonoBehaviour
{
    public int timeNeeded = 5;
    private float currentTimeHeld = 0f;
    public int clicksNeeded = 5;
    private int currentClicks = 0;
    public int nailsNeeded = 4;
    private int currentNails = 0;

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

    private HammerNailController currentNail;

    public AudioSource hammerAudio;
    public AudioClip hammerSound;

    public Camera mainCam;
    public LayerMask nailLayer;

    private Transform newChairPos;

    public UnityEvent<bool> taskCompleted;
    public UnityEvent resetLeg;

    public bool debugBuild = false;
    public GameObject chairObject;

    void Start()
    {
        noise.maxValue = noiseThreshold;

        hammerAudio = GetComponent<AudioSource>();
        hammerAudio.clip = hammerSound;

        //newChairPos = oldChair.transform;
    }

    void Update()
    {
        if (isMinigameActive)
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            currentClicks = currentNail.GetComponent<HammerNailController>().currentClicks;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, nailLayer))
            {
                //Debug.Log("SHOWS CURSOR");
                //Cursor.visible = false;
                //uiCursor.ShowCursor();

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("GOES INTO UPDATE");
                    hammerAudio.PlayOneShot(hammerSound);
                    HandleClick();
                }

                //if (Input.GetMouseButton(0))
                //{
                //    HandleHoldClick();
                //}

                //if (Input.GetMouseButtonUp(0))
                //{
                //    hammerAudio.Stop();
                //}

                // Update the progress slider
                if (currentNail != null)
                {
                    progress.value = currentClicks;
                }
            }
            else
            {
                //uiCursor.HideCursor();
                //Cursor.visible = true;
            }

            currentNoise = Mathf.Max(0f, currentNoise - noiseDecreaseRate * Time.deltaTime);
            noise.value = (noiseThreshold != 0f) ? currentNoise / noiseThreshold : 0f;
            fill.color = gradient.Evaluate(currentNoise);
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
        currentNail = nailPrefab.GetComponent<HammerNailController>();
        minigameUI.SetActive(true);

        Debug.Log("Minigame started");

        // Set the max value of the progress slider
        progress.maxValue = clicksNeeded;
    }

    public void EndMinigame()
    {
        hammerAudio.Stop();
        isMinigameActive = false;
        minigameUI.SetActive(false);

        if (currentNail != null)
        {
            Destroy(currentNail.gameObject);
        }

        currentNail = null;
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
            currentNail.GetComponent<HammerNailController>().currentClicks++;
        }

        if (currentClicks >= clicksNeeded)
        {
            currentNails++;
            EndMinigame();
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
        StartCoroutine(RotatingNew());
        taskCompleted.Invoke(true);
    }

    IEnumerator RotatingNew()
    {
        //yield return new WaitForSeconds(2f);
        //determining rotation for object
        Quaternion targetRotation = Quaternion.Euler(180f, 0f, 0f);
        //determining position for object
        Vector3 startPosition = chairObject.transform.position;
        Vector3 targetPosition = startPosition + Vector3.up * 1f;
        Vector3 currentCamPosition = mainCam.transform.position;
        Vector3 newCamPosition = currentCamPosition + (Vector3.back * 2f) + (Vector3.up * 1f);
        float elapsedTime = 0f;
        float rotationTime = 3f;

        while (elapsedTime < rotationTime)
        {
            //lerp rotation and position of shelfObject
            chairObject.transform.rotation = Quaternion.Lerp(chairObject.transform.rotation, targetRotation, elapsedTime / rotationTime);
            chairObject.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
            mainCam.transform.position = Vector3.Lerp(currentCamPosition, newCamPosition, elapsedTime / rotationTime);
        }
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
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
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
