using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class NailGame : MonoBehaviour
{

    public int clicksNeeded = 25;
    private int currentClicks = 0;

    public int nailsNeeded = 4;
    private int currentNails = 0;

    public float noiseThreshold = 0.7f;
    private float currentNoise = 0f;

    public float noiseIncreaseRate = 0.1f;
    public float noiseDecreaseRate;

    public bool isMinigameActive = false;
    private bool isObjectBuilt = false;

    public UICursor uiCursor;

    public GameObject minigameUI;

    public Slider noise;
    public Slider progress;

    public Image fill;
    public Gradient gradient;

    public GameObject oldChair;
    public GameObject newChair;

    private GameObject currentNail;
    private bool isMuffled = false;

    public SceneTransition sceneTransition;

    public AudioSource hammeringAudio;
    public AudioClip hammerSound;

    [Header("Neighbour Corner")]

    public GameObject player;

    public Transform minigamePos;

    public GameObject topCorner;
    public GameObject bottomCorner;
    private GameObject instantiatedChair;

    public AngerBar clickHandlerReferenceBottomRight;

    public AngerBar clickHandlerReferenceTopRight;

    public GameObject mainCam;
    public GameObject minigameCam;
    public Camera camRay;
    public LayerMask nailLayer;

    public GameObject chairSet;

    public UnityEvent<bool> taskCompleted;
    public UnityEvent resetLeg;

    private Transform newChairPos;


    InventoryManager.AllItems rubberHammer = InventoryManager.AllItems.RubberCover;

    // Start is called before the first frame update
    public void Start()
    {
        
        minigameCam.SetActive(false);

        progress.maxValue = clicksNeeded;
        noise.maxValue = noiseThreshold;
        noiseDecreaseRate = noiseIncreaseRate;

        hammeringAudio = GetComponent<AudioSource>();
        hammeringAudio.clip = hammerSound;

        taskCompleted.AddListener(isTaskComplete => GameObject.FindGameObjectWithTag("MainProgressBar").GetComponent<ProgressBar>().OnTaskCompletion(isTaskComplete));
        GameObject[] draggableObjects = GameObject.FindGameObjectsWithTag("Draggable");

        // Add listeners for each DraggableObject
        foreach (GameObject draggableObject in draggableObjects)
        {
            DraggableObjects draggableScript = draggableObject.GetComponent<DraggableObjects>();

            if (draggableScript != null)
            {
                resetLeg.AddListener(() => draggableScript.ResetObject());
            }
            else
            {
                Debug.LogWarning("No DraggableObjects script found on " + draggableObject.name);
            }
        }
    
        newChairPos = oldChair.transform;

        sceneTransition = FindObjectOfType<SceneTransition>();

        AngerBar clickHandlerReferenceBottomRight = GetComponent<AngerBar>();
        AngerBar clickHandlerReferenceTopRight = GetComponent<AngerBar>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camRay.ScreenPointToRay(Input.mousePosition);

        if (isMinigameActive)
        {
            currentClicks = currentNail.GetComponent<NailObjectController>().currentClicks;

            progress.value = currentClicks;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, nailLayer))
            {

                uiCursor.ShowCursor();
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Im hitting a nail");
                    hammeringAudio.PlayOneShot(hammerSound);
                    // Handle the click only if the hit GameObject is a nail
                    HandleClick();
                }
          
            }
            else
            {
                uiCursor.HideCursor();
            }


            currentNoise = Mathf.Max(0f, currentNoise - noiseDecreaseRate * Time.deltaTime);

            noise.value = (noiseThreshold != 0f) ? currentNoise / noiseThreshold : 0f;

            fill.color = gradient.Evaluate(currentNoise);

        }

    }

    public void StartMinigame(GameObject nailPrefab)
    {

        if (HasHammer() && !isMuffled)
        {
            noiseIncreaseRate *= 0.5f;
            isMuffled = true;
        }
        isMinigameActive = true;
        currentNail = nailPrefab;
        minigameUI.SetActive(true);
        if (currentNail != null)
        {
            currentClicks = currentNail.GetComponent<NailObjectController>().currentClicks;
        }

        minigameCam.SetActive(true);
        mainCam.SetActive(false);


        Debug.Log("Time to build");

    }

    public void EndMinigame()
    {
        
        Debug.Log("You're done building");
        
        minigameUI.SetActive(false);
        
        isMinigameActive = false;
        if (currentNail != null)
        {
            currentNail.SetActive(false);
        }

        

        currentNail = null;

       



        //if (furnitureObjects != null && furnitureObjects.Length > 0)
        //{
        //    int randomIndex = Random.Range(0, furnitureObjects.Length);
        //    GameObject spawnedObject = Instantiate(furnitureObjects[randomIndex], new Vector3(player.transform.position.x, 0, player.transform.position.z), Quaternion.identity);
        //    Debug.Log(spawnedObject.name);
        //}

    }

    public void HandleClick()
    {
        if (currentClicks < clicksNeeded)
        {
            currentNail.GetComponent<NailObjectController>().currentClicks++;

            


            currentNoise = Mathf.Min(currentNoise + noiseIncreaseRate, noiseThreshold);

           
                Debug.Log("im angry1");
                currentNoise = Mathf.Min(currentNoise + noiseIncreaseRate, noiseThreshold);

                if (currentNoise > (noiseThreshold * 0.85f))
                {
                    // Check if the player is inside the collider of topCorner
                    if (IsPlayerInsideGameObject(player, topCorner))
                    {
                        Debug.Log("im angry2");
                        clickHandlerReferenceTopRight.DecreaseAnger();
                        //clickHandlerReferenceTopRight.PromiseRoute();
                    }
                    // Check if the player is inside the collider of bottomCorner
                    else if (IsPlayerInsideGameObject(player, bottomCorner))
                    {
                        clickHandlerReferenceBottomRight.DecreaseAnger();
                        //clickHandlerReferenceBottomRight.PromiseRoute();

                    }

                }
            
        }

        if (currentClicks >= clicksNeeded)
        {
            currentNails++;
            EndMinigame();
            BuildObject();
        }

    }
    void BuildObject()
    {
        if (currentNails == 4 )
        {
            StartCoroutine(DestroyDelay());
            taskCompleted.Invoke(true);
        }

    }

    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(1.25f);

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, 180f);

        float elapsedTime = 0f;
        float rotationTime = 2f; // Adjust this value as needed for the desired rotation time

        while (elapsedTime < rotationTime)
        {
            oldChair.transform.rotation = Quaternion.Lerp(oldChair.transform.rotation, targetRotation, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Imma go disappear");

        oldChair.SetActive(false);

        instantiatedChair = Instantiate(newChair, new Vector3(newChairPos.position.x, newChairPos.position.y - 1f, newChairPos.position.z), transform.rotation);

        yield return new WaitForSeconds(2f);

        Destroy(instantiatedChair);


        minigameCam.SetActive(false);
        mainCam.SetActive(true);

        oldChair.SetActive(true);

        oldChair.transform.rotation = Quaternion.identity;

        currentNails = 0;

        resetLeg.Invoke();
    }

    public bool HasHammer()
    {
        if (InventoryManager.Instance.invItems.Contains(rubberHammer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }



    bool IsPlayerInsideGameObject(GameObject player, GameObject corner)
    {
        Collider cornerCollider = corner.GetComponent<Collider>();

        // Check if the player's position is inside the collider bounds
        return cornerCollider.bounds.Contains(player.transform.position);
    }

    /*private bool ClickNailsInToLeg(Vector3 clickPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(clickPosition);
        RaycastHit hit;

        int layerMask = LayerMask.GetMask("Nails");

        return Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask);
    }*/

}

