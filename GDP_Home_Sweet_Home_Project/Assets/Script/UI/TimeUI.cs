using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI timeText_NotificationBar;
    public TextMeshProUGUI timeText_HomeScreen;


    private void OnEnable()
    {
        TimeController.OnMinuteChanged += UpdateTime;
        TimeController.OnHourChanged += UpdateTime;
    }

    private void OnDisable()
    {
        TimeController.OnMinuteChanged -= UpdateTime;
        TimeController.OnHourChanged -= UpdateTime;
    }

    private void UpdateTime()
    {
        timeText_NotificationBar.text = $"{TimeController.Hour:00}:{TimeController.Minute:00}";
        timeText_HomeScreen.text = $"{TimeController.Hour:00}:{TimeController.Minute:00}";
    }

    private void Start()
    {
       
    }

}
