using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }

    private float minuteToRealTime = 0.5f;
    private float timer;

    [SerializeField]
    private float endHour;
    [SerializeField]
    private float endMinute;

    // Start is called before the first frame update

    void Start()
    {
        Minute = 30;
        Hour = 19;
    }

    // Update is called once per frame
    void Update()
    {
        if (Hour == endHour && Minute == endMinute)
        {
            Debug.Log("Sleep time");
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
            }
        }
    }
}
