using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using TMPro;

public enum TimePhase
{
    Morning,
    Evening,
    QuietTime
}

public class TimeController : MonoBehaviour
{
    public static TimeController instance;

    public static Action OnMinuteChanged;
    public static Action OnHourChanged;

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int CurrentDay { get; private set; }

    public bool isPaused { get; private set; }

    public TimePhase currentTimePhase { get; private set; }
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

    public GameObject LoadingScreenObj;

    [SerializeField] private Slider LoadingBarTimer;

    [Header("Background Sound")]
    public AudioClip morningPhase;
    public AudioClip eveningPhase;
    public AudioClip quietTimePhase;
    private AudioClip currentBGAudioClip;


    [Header("Game Object")]
    [SerializeField] private GameObject firstDay;
    [SerializeField] private GameObject secondDay;
    [SerializeField] private GameObject thirdDay;
    [SerializeField] private GameObject fourthDay;
    [SerializeField] private GameObject fifthDay;

    [Header("Loading Screen UI References")]
    [SerializeField] private Image imageObject;
    [SerializeField] private List<Sprite> loadingImages = new List<Sprite>();
    [SerializeField] private Sprite hakiimImage;
    [SerializeField] private Sprite sherrylImage;
    [SerializeField] private Image dayCycleImage;
    [SerializeField] private Sprite dayImage;
    [SerializeField] private Sprite nightImage;
    [SerializeField] private TextMeshProUGUI dayNumber;
    [SerializeField] private TextMeshProUGUI dayText;

    public LoadingScreen loadingScreen;


    void Start()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        CurrentDay = 1;
        SetTime(startHour, startMinute);
        isPaused = false;
        LoadingScreenObj.SetActive(false);

        if (CurrentDay == 1)
        {
            Debug.Log("Next Morning");
            firstDay.SetActive(true);
        }
        //add images into loadingImages list
        loadingImages.Add(hakiimImage);
        loadingImages.Add(sherrylImage);
    }



    // Update is called once per frame
    void Update()
    {
        currentTimePhase = DetermineCurrentTimePhase();
        

        HandleTime();
        if (Input.GetKeyDown(KeyCode.M))
        {
            AdvanceTimePhase();
        }

        PlayBackGroundMusic(currentTimePhase);
    }

    private void PlayBackGroundMusic(TimePhase phase)
    {
        AudioClip newClip = null;

        switch (phase)
        {
            case TimePhase.Morning:
                newClip = morningPhase;
                break;
            case TimePhase.Evening:
                newClip = eveningPhase;
                break;
            case TimePhase.QuietTime:
                newClip = quietTimePhase;
                break;
        }

        if (newClip != currentBGAudioClip)
        {
            currentBGAudioClip = newClip;
            AudioManager.Instance.PlayBGAudio(currentBGAudioClip);
        }
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

    public void AdvanceTimePhase()
    {
        // immediately end the morning phase
        // call to loading screen
        // set time to evening start time
        //randomize between multiple backgrounds
        if (loadingImages.Count > 0)
        {
            int randomIndex = Random.Range(0, loadingImages.Count); // Get a random index
            Sprite selectedImage = loadingImages[randomIndex]; // Select a random sprite
            imageObject.sprite = selectedImage; // Assign the selected sprite to the Image component
        }

        loadingScreen.ShowRandomMessage();

        if (currentTimePhase == TimePhase.Morning)
        {
            dayCycleImage.sprite = nightImage;
            dayText.SetText("Evening");
            StartCoroutine(LoadingScreenSync());
            SetTime(17, 30);
        }

        if (currentTimePhase == TimePhase.Evening)
        {
            if (CurrentDay == endDay)
            {
                Debug.Log("TimeController - Final day reached, loading End Scene");
                // load end scene
                return;
            }


            dayCycleImage.sprite = dayImage;
            dayText.SetText("Morning");
            StartCoroutine(LoadingScreenSync());
            SetTime(startHour, startMinute);
            CurrentDay++;
            dayNumber.SetText("0" + CurrentDay);
            //Debug.Log("CurrentDay has advanced to: " + CurrentDay);
            PackageSpawnerByDay();
        }


    }

    private void SetTime(int hour, int minute)
    {
        Minute = minute;
        Hour = hour;

        UpdateDirectionalLight();
    }

    private TimePhase DetermineCurrentTimePhase()
    {
        if (Hour >= 6 && Hour <= 8)
        {
            return TimePhase.Morning;

        }
        else if (Hour >= 8 && (Hour < 22 || (Hour == 22 && Minute < 30)))
        {
            return TimePhase.Evening;

        }
        else
        {
            return TimePhase.QuietTime;
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

            isPaused = true;
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
                        isPaused = true;
                        Debug.Log("Time to go to work");
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

    IEnumerator LoadingScreenSync()
    {
        isPaused = true;
        PlayerMovement.dialogue = true;

        Debug.Log("Loading...");
        LoadingScreenObj.SetActive(true);
        LoadingBarTimer.value = 0;

        float loadingTime = 10f;
        float timeGoBy = 0f;

        while (timeGoBy < loadingTime)
        {
            timeGoBy += Time.deltaTime;

            LoadingBarTimer.value = Mathf.Clamp01(timeGoBy / loadingTime); //update the value to the slider and use clamp 01 to clamp the value between 0 and 1 
            yield return null;

        }

        //yield return new WaitForSecondsRealtime(10f);
        LoadingScreenObj.SetActive(false);


        PlayerMovement.dialogue = false;
        isPaused = false;

    }

    private void PackageSpawnerByDay()
    {

        if (CurrentDay == 2)
        {
            secondDay.SetActive(true);
            Debug.Log("Day 2 baby");
        }
        if(CurrentDay == 3)
        {
            thirdDay.SetActive(true);
            Debug.Log("Day 3 MF");
        }
        if(CurrentDay == 4)
        {
            fourthDay.SetActive(true);

        }
        if(CurrentDay == 5)
        {
            fifthDay.SetActive(true);
        }
    }
}

