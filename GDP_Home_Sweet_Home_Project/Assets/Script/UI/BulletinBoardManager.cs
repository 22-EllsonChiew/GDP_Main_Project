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
        UpdateHakimBulletinBoard();
        UpdateSherrylBulletinBoard();
    }

    void UpdateHakimBulletinBoard()
    {
        NeighbourRoutines upcomingRoutine = neighbourHakim.upcomingRoutine;

        if (upcomingRoutine != null)
        {
            if (upcomingRoutine.routineType == RoutineType.NoNoise)
            {
                hakimRoutineText.text = "Please DO NOT make noise from " +
                    upcomingRoutine.routineStartHour + ":" + upcomingRoutine.routineStartMinute + " to "
                    + upcomingRoutine.routineEndHour + ":" + upcomingRoutine.routineEndMinute;
            }

            if (upcomingRoutine.routineType == RoutineType.NotHome)
            {
                hakimRoutineText.text = "I'll be out of my flat from " +
                    upcomingRoutine.routineStartHour + ":" + upcomingRoutine.routineStartMinute + " to "
                    + upcomingRoutine.routineEndHour + ":" + upcomingRoutine.routineEndMinute;
            }
        }
        else
        {
            hakimRoutineText.text = "I'll probably be home all day.";
        }
    }

    void UpdateSherrylBulletinBoard()
    {
        NeighbourRoutines upcomingRoutine = neighbourSherryl.upcomingRoutine;

        if (upcomingRoutine != null)
        {
            if (upcomingRoutine.routineType == RoutineType.NoNoise)
            {
                sherrylRoutineText.text = "Please DO NOT make noise from " +
                    upcomingRoutine.routineStartHour + ":" + upcomingRoutine.routineStartMinute + " to "
                    + upcomingRoutine.routineEndHour + ":" + upcomingRoutine.routineEndMinute;
            }

            if (upcomingRoutine.routineType == RoutineType.NotHome)
            {
                sherrylRoutineText.text = "I'll be out of my flat from " +
                    upcomingRoutine.routineStartHour + ":" + upcomingRoutine.routineStartMinute + " to "
                    + upcomingRoutine.routineEndHour + ":" + upcomingRoutine.routineEndMinute;
            }
        }
        else
        {
            sherrylRoutineText.text = "I'll probably be home all day.";
        }
    }
}
