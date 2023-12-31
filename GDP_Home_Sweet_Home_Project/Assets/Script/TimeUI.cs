using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{

    public TextMeshProUGUI timeText;

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
        timeText.text = $"{TimeController.Hour:00}:{TimeController.Minute:00}";
    }

}
