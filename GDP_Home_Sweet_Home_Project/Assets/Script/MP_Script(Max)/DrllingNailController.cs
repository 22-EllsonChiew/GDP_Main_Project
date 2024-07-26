using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillingNailController : MonoBehaviour
{
    public Camera gameCamera;
    public LayerMask nailLayer;

    public GameObject gameManager;

    private DrillingMiniGame drillingMiniGame;
    public int currentClicks;
    public float currentTimeClicked;

    void Start()
    {
        if (gameManager != null)
        {
            Debug.Log("Game Manager assigned: " + gameManager.name);
            drillingMiniGame = gameManager.GetComponent<DrillingMiniGame>();
            if (drillingMiniGame == null)
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
            Debug.LogError("Game Manager is not assigned.");
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
                    if (drillingMiniGame != null)
                    {
                        Debug.Log("Starting Minigame...");
                        drillingMiniGame.StartMinigame(gameObject);
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
        currentClicks = 0;
        currentTimeClicked = 0;
    }
}