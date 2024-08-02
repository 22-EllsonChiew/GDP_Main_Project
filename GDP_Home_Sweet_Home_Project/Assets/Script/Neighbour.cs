using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbour : MonoBehaviour
{
    public string neighbourName;
    public Transform neighbourTransform;
    public bool IsNeighbourInRoutine {  get; private set; }

    private List<NeighbourRoutines> routines;
    public NeighbourRoutines currentRoutine {  get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        routines = new List<NeighbourRoutines>()
        {
            new NeighbourRoutines() {day = 1, routineStartHour = 18, routineStartMinute = 50, routineEndHour = 20, routineEndMinute = 0, routineType = RoutineType.NotHome }
        };
        neighbourTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // check if neighbour has a routine at specified time
        foreach (var routine in routines)
        {
            if (routine.day == TimeController.CurrentDay)
            {
                if (routine.routineStartHour == TimeController.Hour && routine.routineStartMinute == TimeController.Minute)
                {
                    IsNeighbourInRoutine = true;
                    currentRoutine = routine;
                }
                else if (routine.routineEndHour == TimeController.Hour && routine.routineEndMinute == TimeController.Minute)
                {
                    IsNeighbourInRoutine = false;
                    currentRoutine = null;
                }
            }
            
        }
    }
}
