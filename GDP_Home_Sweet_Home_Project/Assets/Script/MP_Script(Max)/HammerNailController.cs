using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerNailController : MonoBehaviour
{
    public Camera gameCamera;
    public LayerMask nailLayer;

    public GameObject gameManager;

    private RevampedNailGame hammerMiniGame;
    public float currentProgress;
    public int currentClicks;

    void Start()
    {
        if (gameManager != null)
        {
            Debug.Log("Game Manager assigned: " + gameManager.name);
            hammerMiniGame = gameManager.GetComponent<RevampedNailGame>();
            if (hammerMiniGame == null)
            {
                Debug.LogError("DrillingMiniGame component not found on Game Manager.");
            }
            else
            {
                Debug.Log("DrillingMiniGame component successfully assigned.");
            }
        }
        else
        {
            Debug.LogError("Game Manager is not assigned." + this.name);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, nailLayer))
            {
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    Debug.Log("Hit smth");
                    if (hammerMiniGame != null)
                    {
                        Debug.Log("Starting Minigame...");
                        hammerMiniGame.StartMinigame(gameObject);
                    }
                    else
                    {
                        Debug.LogError("DrillingMiniGame component is not assigned.");
                    }
                }
            }
        }
    }

    public void ResetCount()
    {
        Debug.Log("Reset count");
        currentProgress = 0;
        currentClicks = 0;
    }
}