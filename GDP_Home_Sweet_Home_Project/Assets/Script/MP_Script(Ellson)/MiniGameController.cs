using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;


public class MiniGameController : MonoBehaviour
{
    [Header("Game Handler")]
    [SerializeField] private int clicksNeedsToHammer = 4;
    [SerializeField] private int currentClicks = 0;

    [SerializeField] private int nailsNeed = 4;
    [SerializeField] private int currentNails = 0;

    private GameObject currentNail;
    private bool isInMiniGame = false;

    [SerializeField] private UICursor uiCursor;

    

    [Header("GameObject")]

    [SerializeField] private GameObject polyChair;
    [SerializeField] private GameObject newChair;


    private GameObject instantiatedChair;
    private Transform newChairPos;

    [Header("Camera")]

    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject minigameCam;
    [SerializeField] private Camera camRay;
    public LayerMask nailLayer;

    public GameObject chairSet;

    public UnityEvent<bool> taskCompleted;
    public UnityEvent resetLeg;

    // Start is called before the first frame update
    void Start()
    {
        minigameCam.SetActive(false);

       // taskCompleted.AddListener(isTaskComplete => GameObject.FindGameObjectWithTag("MainProgressBar").GetComponent<ProgressBar>().OnTaskCompletion(isTaskComplete));

        GameObject[] draggableObjects = GameObject.FindGameObjectsWithTag("Draggable");

        foreach (GameObject draggableObject in draggableObjects)
        {
            DraggableObjects draggableScript = draggableObject.GetComponent<DraggableObjects>();

            if (draggableScript != null)
            {
                Debug.Log("yes");
                resetLeg.AddListener(() => draggableScript.ResetObject());
            }
            else
            {
                Debug.LogWarning("No DraggableObjects script found on " + draggableObject.name);
            }
        }


        newChairPos = polyChair.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camRay.ScreenPointToRay(Input.mousePosition);

        if(isInMiniGame)
        {
            currentClicks = currentNail.GetComponent<NailObjectController>().currentClicks;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, nailLayer))
            {
                Cursor.visible = false;
                uiCursor.ShowCursor();
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Im hitting a nail");
                    
                    // Handle the click only if the hit GameObject is a nail
                    HandleClick();
                }

            }
            else
            {

                uiCursor.HideCursor();
                Cursor.visible = true;
            }
        }
    }

    public void StartMinigame(GameObject nailPrefab)
    {
        GameObject[] nails = GameObject.FindGameObjectsWithTag("Nail");

        foreach (GameObject nail in nails)
        {
            NailObjectController nailScript = nail.GetComponent<NailObjectController>();

            if (nailScript != null)
            {
                Debug.Log("hehe");
                resetLeg.AddListener(() => nailScript.ResetCount());
            }
        }




        isInMiniGame = true;
        currentNail = nailPrefab;
       
        if (currentNail != null)
        {
            currentClicks = currentNail.GetComponent<NailObjectController>().currentClicks;
        }

        minigameCam.SetActive(true);
        mainCam.SetActive(false);


        Debug.Log("Time to build");

    }

    public void HandleClick()
    {
        if(currentClicks < clicksNeedsToHammer)
        {
            currentNail.GetComponent<NailObjectController>().currentClicks++;
        }

        if(currentClicks >= clicksNeedsToHammer)
        {
            currentNails++;
            EndMiniGame();
            BuildObject();
        }
    }

    void BuildObject()
    {
        if (currentNails == 4)
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
            polyChair.transform.rotation = Quaternion.Lerp(polyChair.transform.rotation, targetRotation, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Imma go disappear");

        polyChair.SetActive(false);

        instantiatedChair = Instantiate(newChair, new Vector3(newChairPos.position.x, newChairPos.position.y - 1f, newChairPos.position.z), transform.rotation);

        yield return new WaitForSeconds(2f);

        Destroy(instantiatedChair);


        minigameCam.SetActive(false);
        mainCam.SetActive(true);

        polyChair.SetActive(true);

        polyChair.transform.rotation = Quaternion.identity;

        currentNails = 0;

        resetLeg.Invoke();
    }

    public void EndMiniGame()
    {
        Debug.Log("You're done building");

        if (currentNail != null)
        {
            currentNail.SetActive(false);
        }

        currentNail = null;

        Cursor.visible = true;

    }
}
