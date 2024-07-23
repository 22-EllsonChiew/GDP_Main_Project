using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillingNailController : MonoBehaviour
{
    public Camera gameCamera;
    public LayerMask nailLayer;

    // Add this field to reference the Game Manager
    public GameObject gameManager;

    private DrillingMiniGame drillingMiniGame;

    void Start()
    {
        // Ensure the gameManager reference is assigned
        if (gameManager != null)
        {
            drillingMiniGame = GetComponent<DrillingMiniGame>();
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
                        drillingMiniGame.StartMinigame(gameObject);
                        Debug.Log("Starting Hammering");
                    }
                    else
                    {
                        Debug.LogError("DrillingMiniGame component is not assigned.");
                    }
                }
            }
        }
    }
}