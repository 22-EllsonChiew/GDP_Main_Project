using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

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

    private bool isMinigameActive = false;
    private bool isObjectBuilt = false;

    public GameObject minigameUI;

    public Slider noise;
    public Slider progress;

    public Image fill;
    public Gradient gradient;

    public GameObject oldChair;
    public GameObject newChair;

    private GameObject currentNail;

    public SceneTransition sceneTransition;

    [Header("Neighbour Corner")]

    public GameObject player;

    public GameObject topCorner;
    public GameObject bottomCorner;

    public AngerBar clickHandlerReferenceBottomRight;

    public AngerBar clickHandlerReferenceTopRight;


    // Start is called before the first frame update
    public void Start()
    {
        progress.maxValue = clicksNeeded;
        noise.maxValue = noiseThreshold;
        noiseDecreaseRate = noiseIncreaseRate;


        sceneTransition = FindObjectOfType<SceneTransition>();

        AngerBar clickHandlerReferenceBottomRight = GetComponent<AngerBar>();
        AngerBar clickHandlerReferenceTopRight = GetComponent<AngerBar>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isMinigameActive)
        {
            currentClicks = currentNail.GetComponent<NailObjectController>().currentClicks;
                
            if (Input.GetMouseButtonDown(0))
            {
                HandleClick();
            }

            currentNoise = Mathf.Max(0f, currentNoise - noiseDecreaseRate * Time.deltaTime);

            noise.value = (noiseThreshold != 0f) ? currentNoise / noiseThreshold : 0f;

            fill.color = gradient.Evaluate(currentNoise);
        }


    }

    public void StartMinigame(GameObject nailPrefab)
    {

        //if (HasHammer() && !isMuffled)
        //{
        //    noiseIncreaseRate *= 0.5f;
        //    isMuffled = true;
        //}
        isMinigameActive = true;
        currentNail = nailPrefab;
        minigameUI.SetActive(true);
        ResetMinigame();


        Debug.Log("Time to build");

    }

    private void ResetMinigame()
    {
        if (currentNail != null)
        {
            currentClicks = currentNail.GetComponent<NailObjectController>().currentClicks;
        }
        //add completion bar for game
    }

    public void EndMinigame()
    {
        
        Debug.Log("You're done building");
        
        minigameUI.SetActive(false);
        
        isMinigameActive = false;
        if (currentNail != null)
        {
            Destroy(currentNail);
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

            progress.value = currentClicks;


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
                    }
                    // Check if the player is inside the collider of bottomCorner
                    else if (IsPlayerInsideGameObject(player, bottomCorner))
                    {
                        clickHandlerReferenceBottomRight.DecreaseAnger();
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
        }

    }

    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(1.25f);

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, 180f);
        Transform chairPos = oldChair.transform;

        float elapsedTime = 0f;
        float rotationTime = 2f; // Adjust this value as needed for the desired rotation time

        while (elapsedTime < rotationTime)
        {
            oldChair.transform.rotation = Quaternion.Lerp(oldChair.transform.rotation, targetRotation, elapsedTime / rotationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Imma go disappear");
        Destroy(oldChair);

        GameObject instantiatedChair = Instantiate(newChair, new Vector3(chairPos.position.x, chairPos.position.y - 0.3f, chairPos.position.z), transform.rotation);
        instantiatedChair.transform.localScale = new Vector3(0.275f, 0.275f, 0.275f);

       

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

