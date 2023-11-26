using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class HammerMinigame : MonoBehaviour
{

    public int clicksNeeded = 25;
    private int currentClicks = 0;

    public float noiseThreshold = 0.7f;
    private float currentNoise = 0f;

    public float noiseIncreaseRate = 0.1f;
    public float noiseDecreaseRate = 0.05f;

    public int neighbourTotalHealth = 5;



    private bool isMinigameActive = false;


    public GameObject player;
    public GameObject gameUI;
    public GameObject minigameUI;

    public Slider noise;
    public Slider progress;

    public Image fill;
    public Gradient gradient;


    public Camera mainCamera;
    public float minigameFOV = 20f;
    public float fovChangeDuration = 2f;
    private float originalFOV;
    private bool swapFOV = false;
    private float fovChangeStartTime;


    void Start()
    {
        FindObjectOfType<Interaction>().OnTaskInteract += StartMinigame;
        originalFOV = mainCamera.fieldOfView;
        progress.maxValue = clicksNeeded;
        noise.maxValue = noiseThreshold;

    }

    private void StartMinigame(bool isTaskStarted)
    {
        if (isTaskStarted)
        {
            isMinigameActive = true;
            gameUI.SetActive(false);
            minigameUI.SetActive(true);
            ResetMinigame();

            fovChangeStartTime = Time.time;

            player.GetComponent<PlayerMovement>().enabled = false;

            Debug.Log("Time to build");
        }
    }

    private void ResetMinigame()
    {
        currentClicks = 0;
        //add completion bar for game
        currentNoise = 0f;
    }

    private void EndMinigame()
    {
        Debug.Log("You're done building");
        gameUI.SetActive(true);
        minigameUI.SetActive(false);
        isMinigameActive = false;
        player.GetComponent<PlayerMovement>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {

        ChangeFOV();


        if (isMinigameActive)
        {
           
            if (Input.GetMouseButtonDown(0))
            {
                HandleClick();
            }

            currentNoise = Mathf.Max(0f, currentNoise - noiseDecreaseRate * Time.deltaTime);

            noise.value = (noiseThreshold != 0f) ? currentNoise / noiseThreshold : 0f;

            fill.color = gradient.Evaluate(currentNoise);

        }


    }

    private void HandleClick()
    {
        if (currentClicks < clicksNeeded)
        {
            currentClicks++;

            progress.value = currentClicks;

            currentNoise = Mathf.Min(currentNoise + noiseIncreaseRate, noiseThreshold);

            if (currentNoise > (noiseThreshold * 0.85f))
            {
                neighbourTotalHealth -= 1;
            }

            if (currentClicks >= clicksNeeded)
            {
                EndMinigame();
            }
        }
    }

    private void ChangeFOV()
    {

        //if (isMinigameActive)
        //{
        //    float elapsedChangeTime = Time.time - fovChangeStartTime;

        //    float t = Mathf.Clamp01(elapsedChangeTime / fovChangeDuration);

        //    mainCamera.fieldOfView = Mathf.Lerp(originalFOV, minigameFOV, t);

        //    if (t > 1.0f)
        //    {
        //        swapFOV = false;
        //    }
        //}
        //else if (mainCamera.fieldOfView != originalFOV)
        //{
        //    fovChangeStartTime = Time.time;

        //    float elapsedChangeTime = Time.time - fovChangeStartTime;

        //    float t = Mathf.Clamp01(elapsedChangeTime / fovChangeDuration);

        //    mainCamera.fieldOfView = Mathf.Lerp(minigameFOV, originalFOV, t);

        //}

        float targetFOV = isMinigameActive ? minigameFOV : originalFOV;

        float elapsedChangeTime = Time.time - fovChangeStartTime;
        float t = Mathf.Clamp01(elapsedChangeTime / fovChangeDuration);

        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, t);

        if (!isMinigameActive && t >= 1.0f && mainCamera.fieldOfView != originalFOV)
        {
            // Ensure we only run this code when transitioning back to original FOV
            mainCamera.fieldOfView = originalFOV;
        }

    }

}   
