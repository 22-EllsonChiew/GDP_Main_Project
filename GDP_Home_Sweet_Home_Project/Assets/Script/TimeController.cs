using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int CurrentDay { get; private set; }

    public static bool isPaused { get; private set; }

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
        LoadingScreenObj.SetActive(false);
    }

    

    // Update is called once per frame
    void Update()
    {
       
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


    private void NightTime()
    {
        Minute = 00;
        Hour = 19;
        UpdateDirectionalLight();
    }


    private void SetTime(int hour, int minute)
    {
        Minute = minute;
        Hour = hour;

        UpdateDirectionalLight();
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

            Debug.Log("Day finished");
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
                        // stop time here, make player move to elevator to proceed day
                        // possibly make use of Interact script, prevent player from building/interacting etc.
                        // for now, force a loading screen
                        StartCoroutine(LoadingScreenSync());
                        SetTime(17, 30);
                    }

                    if (Hour == 24)
                    {
                        CurrentDay++;
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

    //private void NightTimmerController()
    //{
    //    // Check if the current time matches the specified end hour and minute
    //    if (Hour == endHour && Minute == endMinute)
    //    {
    //        // Log a message indicating it's sleep time
    //        Debug.Log("Sleep time");
    //        // Load the scene named "Sleep Scene"
    //        SceneManager.LoadScene("Sleep Scene");
    //    }
    //    else
    //    {
    //        // Decrease the timer by the time elapsed since the last frame
    //        timer -= Time.deltaTime;

    //        // If the timer has reached zero or less, increment the minute
    //        if (timer <= 0)
    //        {
    //            // Increment the minute
    //            Minute++;

    //            // Invoke the OnMinuteChanged event 
    //            OnMinuteChanged?.Invoke();

    //            // Check if the minute has reached 60, indicating an hour has passed
    //            if (Minute >= 60)
    //            {
    //                // Increment the hour
    //                Hour++;

    //                // If the hour has reached 24, reset it to 0 (midnight)
    //                if (Hour == 24)
    //                {
    //                    Hour = 00;
    //                }
                    
                    
    //                // Reset the minute to 0
    //                Minute = 0;
    //                OnHourChanged?.Invoke(); // Invoke the OnHourChanged event

    //            }
    //            // Reset the timer to the duration of one minute in real time
    //            timer = minuteToRealTime;
    //            UpdateDirectionalLight();
    //        }
    //    }
    //}

    //private void DayTimmerController()
    //{
    //    // Check if the current time matches the specified end hour and minute
    //    if (Hour == endHour && Minute == endMinute)
    //    {
    //        // Log a message indicating it's sleep time
    //        Debug.Log("Sleep time");
    //        // Load the scene named "Sleep Scene"
    //        SceneManager.LoadScene("Sleep Scene");
    //    }
    //    else
    //    {
    //        // Decrease the timer by the time elapsed since the last frame
    //        timer -= Time.deltaTime;

    //        // If the timer has reached zero or less, increment the minute
    //        if (timer <= 0)
    //        {
    //            // Increment the minute
    //            Minute++;

    //            // Invoke the OnMinuteChanged event 
    //            OnMinuteChanged?.Invoke();

    //            // Check if the minute has reached 60, indicating an hour has passed
    //            if (Minute >= 60)
    //            {
    //                // Increment the hour
    //                Hour++;

    //                // If the hour has reached 8, reset it to 0 (midnight)
    //                if (Hour == 8)
    //                {
    //                    Hour = 00;

    //                    StartCoroutine(LoadingScreenSync());
    //                    //LoadingScreenObj.SetActive(true);
    //                    NightTime();
    //                    NightTimmerController();

    //                }
    //                // Reset the minute to 0
    //                Minute = 0;
    //                OnHourChanged?.Invoke(); // Invoke the OnHourChanged event

    //            }
    //            // Reset the timer to the duration of one minute in real time
    //            timer = minuteToRealTime;
    //            UpdateDirectionalLight();
    //        }   
    //    }
    //}

    public IEnumerator LoadingScreenSync()
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

