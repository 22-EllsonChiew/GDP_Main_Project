using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletinBoardManager : MonoBehaviour
{

    public Neighbour neighbourHakim;
    public Neighbour neighbourSherryl;

    public TextMeshProUGUI hakimRoutineText;
    public TextMeshProUGUI sherrylRoutineText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBulletinBoard(neighbourHakim, hakimRoutineText);
        UpdateBulletinBoard(neighbourSherryl, sherrylRoutineText);
    }

    void UpdateBulletinBoard(Neighbour neighbour, TextMeshProUGUI routineText)
    {
        NeighbourRoutines upcomingRoutine = neighbour.upcomingRoutine;

        if (upcomingRoutine != null)
        {
            switch (upcomingRoutine.routineType)
            {
                case RoutineType.NoNoise:
                    routineText.text = "Please try to keep things quiet between " +
                        FormatTime(upcomingRoutine.routineStartHour, upcomingRoutine.routineStartMinute) + " and " +
                        FormatTime(upcomingRoutine.routineEndHour, upcomingRoutine.routineEndMinute) + ". Thanks!";
                    break;

                case RoutineType.NotHome:
                    routineText.text = "I'll be out from " +
                        FormatTime(upcomingRoutine.routineStartHour, upcomingRoutine.routineStartMinute) + " to " +
                        FormatTime(upcomingRoutine.routineEndHour, upcomingRoutine.routineEndMinute) + ". See you later!";
                    break;

                default:
                    routineText.text = "I'll be busy doing some of my own things.";
                    break;
            }
        }
        else
        {
            routineText.text = "I'll likely be around all day.";
        }
    }

    string FormatTime(int hour, int minute)
    {
        return hour.ToString("00") + ":" + minute.ToString("00");
    }

}
