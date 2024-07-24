using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DrillingMiniGame : MonoBehaviour
{
    public int clicksNeeded = 25;
    public int timeNeeded = 20;
    private int currentClicks = 0;
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

    private GameObject currentNail;
    private bool isMuffled = false;

    public AudioSource drillingAudio;
    public AudioClip drillSound;

    public Camera mainCam;
    public LayerMask nailLayer;

    private GameObject instantiatedChair;
    private Transform newChairPos;

    public UnityEvent<bool> taskCompleted;
    public UnityEvent resetLeg;

    InventoryManager.AllItems rubberHammer = InventoryManager.AllItems.RubberCover;

    void Start()
    {
        progress.maxValue = clicksNeeded;
        noise.maxValue = noiseThreshold;

        drillingAudio = GetComponent<AudioSource>();
        drillingAudio.clip = drillSound;

        newChairPos = oldChair.transform;
    }

    void Update()
    {
        if (isMinigameActive)
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            currentClicks = currentNail.GetComponent<DrillingNailController>().currentClicks;
            progress.value = currentClicks;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, nailLayer))
            {
                Cursor.visible = false;
                uiCursor.ShowCursor();

                //if (Input.GetMouseButtonDown(0))
                //{
                //    hammeringAudio.PlayOneShot(hammerSound);
                //    HandleClick();
                //}
                if (Input.GetMouseButton(0))
                {
                    drillingAudio.PlayOneShot(drillSound);
                    HandleHoldClick();
                }
            }
            else
            {
                uiCursor.HideCursor();
                Cursor.visible = true;
            }

            currentNoise = Mathf.Max(0f, currentNoise - noiseDecreaseRate * Time.deltaTime);
            noise.value = (noiseThreshold != 0f) ? currentNoise / noiseThreshold : 0f;
            fill.color = gradient.Evaluate(currentNoise);
            Debug.Log(currentTimeHeld);
        }
    }

    public void StartMinigame(GameObject nailPrefab)
    {
        //if (HasHammer() && !isMuffled)
        //{
        //    noiseIncreaseRate *= 0.5f;
        //    isMuffled = true;
        //}

        if (nailPrefab == null)
        {
            Debug.LogError("nailPrefab is null.");
            return;
        }

        noiseDecreaseRate = noiseIncreaseRate * 2.25f;
        isMinigameActive = true;
        currentNail = nailPrefab;
        minigameUI.SetActive(true);

        if (currentNail != null)
        {
            currentClicks = currentNail.GetComponent<DrillingNailController>().currentClicks;
        }
        Debug.Log("Minigame started");
    }

    public void EndMinigame()
    {
        isMinigameActive = false;
        minigameUI.SetActive(false);

        if (currentNail != null)
        {
            currentNail.SetActive(false);
        }

        currentNail = null;
        Cursor.visible = true;

        Debug.Log("Minigame ended");
    }

    public void HandleClick()
    {
        if (currentClicks < clicksNeeded)
        {
            currentNail.GetComponent<DrillingNailController>().currentClicks++;

            if (hitParticles != null)
            {
                Instantiate(hitParticles, currentNail.transform.position, Quaternion.identity);
            }

            currentNoise = Mathf.Min(currentNoise + noiseIncreaseRate, noiseThreshold);

            
        }
        if (currentClicks >= clicksNeeded)
        {
            Debug.Log("Finished building");
            EndMinigame();
            BuildObject();
        }
    }

    public void HandleHoldClick()
    {
        currentTimeHeld += Time.deltaTime;
        if (currentTimeHeld < timeNeeded)
        {
            currentNail.GetComponent<DrillingNailController>().currentTimeClicked += Mathf.FloorToInt(currentTimeHeld);
        }
        if (currentTimeHeld >= timeNeeded)
        {
            Debug.Log("FINISHED");
            EndMinigame();
            BuildObject();
        }
    }

    void BuildObject()
    {
        StartCoroutine(DestroyDelay());
        taskCompleted.Invoke(true);
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

        instantiatedChair = Instantiate(newChair, new Vector3(newChairPos.position.x, newChairPos.position.y - 1f, newChairPos.position.z), transform.rotation);

        yield return new WaitForSeconds(2f);

        Destroy(instantiatedChair);

        oldChair.SetActive(true);
        oldChair.transform.rotation = Quaternion.identity;

        resetLeg.Invoke();
    }

    //public bool HasHammer()
    //{
    //    return InventoryManager.Instance.invItems.Contains(rubberHammer);
    //}

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