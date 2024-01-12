using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }

    private float minuteToRealTime = 0.75f;
    private float timer;

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

    // Start is called before the first frame update

    void Start()
    {
        Minute = 30;
        Hour = 19;
        UpdateDirectionalLight();
    }

    // Update is called once per frame
    void Update()
    {
        if (Hour == endHour && Minute == endMinute)
        {
            Debug.Log("Sleep time");
            SceneManager.LoadScene("Sleep Scene");
        }
        else
        {
            timer -= Time.deltaTime;


            if (timer <= 0)
            {
                Minute++;
                OnMinuteChanged?.Invoke();

                if (Minute >= 60)
                {
                    Hour++;

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



}

