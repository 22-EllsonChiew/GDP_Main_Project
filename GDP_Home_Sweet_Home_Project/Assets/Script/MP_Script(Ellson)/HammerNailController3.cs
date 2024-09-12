using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerNailController3 : MonoBehaviour
{
    [SerializeField] public Camera gameCamera;
    [SerializeField] public LayerMask nailLayer;

    [SerializeField] public GameObject gameManager;

    [SerializeField] public RevampNail3 nail3;
    [SerializeField] public float currentProgress;
    [SerializeField] public int currentClicks;

    [Header("Script")]
    [SerializeField] public NoiseController noiseController;
    [SerializeField] public WindowController windowControllers;
    [SerializeField] public PackageRaycast packageRaycast;

    void Start()
    {
        if (gameManager != null)
        {
            Debug.Log("Game Manager assigned: " + gameManager.name);
            nail3 = gameManager.GetComponent<RevampNail3>();
            if (nail3 == null)
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
                    if (nail3 != null)
                    {

                        Debug.Log("Starting Minigame...");
                        nail3.StartMinigame(gameObject);
                        HammeringNoise();
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

    public void HammeringNoise()
    {
        Debug.Log("Time to make some noise!");


        if (packageRaycast.OnCarpet())
        {

            Debug.Log("building on carpet with window close");
            noiseController.MakeNoise(windowControllers.NoiseLevel() - 0.15f);
            noiseController.HandleNoise();

        }
        else if (packageRaycast.OnCarpet())
        {
            Debug.Log("On Carpet but window is open");
            noiseController.MakeNoise(windowControllers.NoiseLevel());
            noiseController.HandleNoise();
        }
        else
        {
            noiseController.MakeNoise(windowControllers.NoiseLevel());
            noiseController.HandleNoise();
        }
    }
}
