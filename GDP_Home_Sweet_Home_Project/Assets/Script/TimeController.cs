using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TimePhase
{
    Morning,
    Evening
}

public class TimeController : MonoBehaviour
{
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int CurrentDay { get; private set; }

    public static bool isPaused { get; private set; }

    public static TimePhase currentTimePhase { get; private set; }
    public static bool hasCompletedTimeSegment { get; private set; }
    private float minuteToRealTime = 0.75f;
    private float timer;

    [SerializeField]
    private int endDay;

    [SerializeField]
    private int startHour;
    [SerializeField]
    private int startMinute;

    [SerializeField]
    private float endHour;
    [SerializeField]
    private float endMinute;

    [SerializeField]
    private Light directionalLight;
    [SerializeField]
    private float dayIntensity = 1.0f;
    [SerializeField]
    private float nightIntensity = 0.2f;
    [SerializeField]
    private float eveningIntensity = 0.5f;
    [SerializeField]
    private float transitionSpeed = 0.1f;


    // Initial rotation values
    private float initialRotationX = -70f;
    private float initialRotationY = 173f;
    private float initialRotationZ = 183f;

    public TimeUI timeUI;

    public GameObject LoadingScreenObj;

    private bool isNight = false;

    private bool isDay = false;

    [SerializeField] private Slider LoadingBarTimer;

    
    void Start()
    {
        CurrentDay = 1;
        SetTime(startHour, startMinute);
        isPaused = false;
        hasCompletedTimeSegment = false;
        LoadingScreenObj.SetActive(false);
    }

    

    // Update is called once per frame
    void Update()
    {
       
        if (hasCompletedTimeSegment)
        {
            isPaused = true;
        }

        currentTimePhase = DetermineCurrentTimePhase();
        HandleTime();
        
    }

    private void UpdateDirectionalLight()
    {
        float t = ((float)Hour + (float)Minute / 60f) / 24f;

        // Adjust intensity based on time
        if (Hour > endHour || (Hour == endHour && Minute >= endMinute)) // Evening time
        {
            directionalLight.intensity = Mathf.Lerp(eveningIntensity, nightIntensity, t);
        }
        else // Daytime
        {
            directionalLight.intensity = Mathf.Lerp(dayIntensity, eveningIntensity, t);
        }

        // Adjust rotation based on time
        float angle = Mathf.Lerp(0f, -360f, t);

        directionalLight.transform.rotation = Quaternion.Euler(initialRotationX + angle, initialRotationY, initialRotationZ);



    }

    public static void EndMorningPhase()
    {
        // immediately end the morning phase
        // call to loading screen
        // set time to evening start time
    }

    public static void EndEveningPhase()
    {
        // immediately end evening phase
        // end current day and move to next
        // set time to next morning

        CurrentDay++;
    }

    private void SetTime(int hour, int minute)
    {
        Minute = minute;
        Hour = hour;

        UpdateDirectionalLight();
    }

    private TimePhase DetermineCurrentTimePhase()
    {
        if (Hour >= 6 && Hour == 8)
        {
            return TimePhase.Morning;
        }
        else
        {
            return TimePhase.Evening;
        }
    }

    private void HandleTime()
    {
        if (Hour == endHour && Minute == endMinute)
        {
            if (CurrentDay == endDay)
            {
                Debug.Log("Total game days reached!");
                // move to end scene
                // housewarming party!
            }

            hasCompletedTimeSegment = true;
            // load into day end scene
            // possibly make use of dream scene here
        }
        else if (!isPaused)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Minute++;

                OnMinuteChanged?.Invoke();

                if (Minute >= 60)
                {
                    Hour++;

                    if (Hour == 8)
                    {
                        hasCompletedTimeSegment = true;
                    }

                    if (Hour == 24)
                    {
                        Hour = 00;
                    }

                    Minute = 0;
                    OnHourChanged?.Invoke();
                }

                timer = minuteToRealTime;
                UpdateDirectionalLight();
            }
        }

    }

    void HandleTimeSegmentTransition()
    {
        // 
    }

    IEnumerator LoadingScreenSync()
    {
        isPaused = true;

        Debug.Log("Loading...");
        LoadingScreenObj.SetActive(true);
        LoadingBarTimer.value = 0;

        float loadingTime = 10f;
        float timeGoBy = 0f;

        while(timeGoBy < loadingTime)
        {
            timeGoBy += Time.deltaTime;

            LoadingBarTimer.value = Mathf.Clamp01(timeGoBy / loadingTime); //update the value to the slider and use clamp 01 to clamp the value between 0 and 1 
            yield return null;

        }

        //yield return new WaitForSecondsRealtime(10f);
        LoadingScreenObj.SetActive(false);

        isPaused = false;
        
    }
}

