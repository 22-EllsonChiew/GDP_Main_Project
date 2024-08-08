using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbour : MonoBehaviour
{
    public string neighbourName;
    public Transform neighbourTransform;

    public float maxHappiness;

    public float happinessThreshold_Normal {  get; private set; }
    public float happinessThreshold_Angry { get; private set; }
    public float complaintThreshold { get; private set; }
    public float currentHappiness { get; private set; }
    public DialogueType currentMood { get; private set; }
    public bool IsNeighbourInRoutine {  get; private set; }

    private List<NeighbourRoutines> routines;
    public NeighbourRoutines currentRoutine {  get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        currentHappiness = maxHappiness;
        happinessThreshold_Normal = maxHappiness * 0.65f;
        happinessThreshold_Angry = maxHappiness * 0.35f;

        complaintThreshold = happinessThreshold_Normal;

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

        CalculateCurrentMood();

    }

    public void EscalateNeighbourComplaint()
    {
        if (complaintThreshold != happinessThreshold_Angry)
        {
            complaintThreshold = happinessThreshold_Angry;
            Debug.Log("neighbour has made a complaint - less forgiving to player");
        }
    }

    public void ReduceHappiness(float amount)
    {
        currentHappiness -= amount;
        currentHappiness = Mathf.Clamp(currentHappiness, 0, maxHappiness);
    }

    void CalculateCurrentMood()
    {
        if (currentHappiness <= happinessThreshold_Angry)
        {
            currentMood = DialogueType.Mood_Angry;
        }
        else if (currentHappiness <= happinessThreshold_Normal)
        {
            currentMood = DialogueType.Mood_Normal;
        }
        else 
        {
            currentMood = DialogueType.Mood_Happy;
        }
    }

}
